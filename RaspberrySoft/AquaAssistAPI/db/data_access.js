var SensorDefinitionModel = require("./../models/SensorDefinition.js");
var RelayDefinitionModel = require("./../models/RelayDefinition.js");

const { Pool, Client } = require('pg')

const pool = new Pool({
  user: 'postgres',
  host: '192.168.103',
  database: 'AquaAssist',
  password: 'varasfinis',
  port: 5432,
})

var testFunction = function() {
  pool.query('SELECT NOW()', (err, res) => {
    console.log(err, res)
    pool.end()
  })
}

// Sensor Definition

var readSensorDefinitions = function(callback){  
  pool.query('SELECT * FROM sensors.sensor_definition', (err, res) => {                             
    callback(SensorDefinitionModel.dbResultToSensorDefinitionArray(res));
  });  
}

var readSensorDefinitionById = function(id, callback){  
  var text = 'SELECT * FROM sensors.sensor_definition WHERE id = $1';
  var values = [id];
  pool.query(text, values, (err, res) => {                         
    callback(SensorDefinitionModel.dbResultToSensorDefinition(res));
  });  
}

var readSensorValuesBySensorId = function(id, callback){  
  var text = 'SELECT * FROM sensors.sensor_data WHERE sensor_id = $1';
  var values = [id];
  pool.query(text, values, (err, res) => {                       
    callback(SensorDefinitionModel.dbResultToSensorValueArray(res));
  });  
}

var readSensorValues = function(sensorId,startDate, endDate, maxEntry, callback){  
  var text = 'select * from sensors.get_sensor_data($1, $2, $3, $4)';  
  var values = [sensorId, startDate, endDate, maxEntry]; 
  console.log(values); 
  pool.query(text, values, (err, res) => {  
    if (err) {
      callback(err);      
    }                     
    callback(SensorDefinitionModel.dbResultToSensorValueArray(res));
  });  
}

var readTopNSensorValues = function(sensorId, n, callback){  
  var text = 'select * from sensors.sensor_data where sensor_id = $1 and ts <= now() order by ts desc limit $2';  
  var values = [sensorId, n]; 
  console.log(values); 
  pool.query(text, values, (err, res) => {  
    if (err) {
      callback(err);      
    }                     
    callback(SensorDefinitionModel.dbResultToSensorValueArray(res));
  });  
}

// Relay Definition

var readRelayDefinitions = function(callback){  
  pool.query('SELECT * FROM relays.relay_data', (err, res) => {                             
    callback(RelayDefinitionModel.dbResultToRelayDefinitionArray(res));
  });  
}

var changeRelayState = function(relayId, callback){
  var text = 'update relays.relay_data set state = not state, last_status_change_ts = now() where id = $1';
  var values = [relayId];
  pool.query(text, values, (err, res)=>{
    if (err){
      callback(err);
    }else{
      callback(RelayDefinitionModel.dbResultToRelayDefinition(res.rows[0]));
    }
  });
}


module.exports = {
  testFunction,
  readSensorDefinitions,
  readSensorDefinitionById,
  readSensorValuesBySensorId,
  readSensorValues,
  readTopNSensorValues,

  readRelayDefinitions
}