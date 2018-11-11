var SensorDefinitionModel = require("./../models/SensorDefinition.js")

const { Pool, Client } = require('pg')

const pool = new Pool({
  user: 'postgres',
  host: '127.0.0.1',
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

module.exports = {
  testFunction,
  readSensorDefinitions,
  readSensorDefinitionById,
  readSensorValuesBySensorId
}