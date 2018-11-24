var SensorDefinitionModel = require("./../models/SensorDefinition.js");
var RelayDefinitionModel = require("./../models/RelayDefinition.js");

const { Pool, Client } = require('pg')

const pool = new Pool({
  user: 'postgres',
  host: '127.0.0.1',
  database: 'AquaAssist',
  password: 'varasfinis',
  port: 5432,
})

// Executes a query with parameters
var RunQuery = function(queryText, queryParams, mappingFunc, callback){
  pool.query(queryText, queryParams, (err, res) => {
    if (err) {
      console.log('Query Error');
      callback(err, null);
    }
    else
    {
      try {
        if (mappingFunc != null) {
          var transformedData = mappingFunc(res);
          callback(null, transformedData);
        } else {          
          callback(null, null)
        }
      } catch(ex) {
        console.log('Transform Error');
        callback(ex, null);
      }      
    }      
  })
}

// Sensor Definition
var readSensorDefinitions = function(query, callback){  
  if (query.id == undefined)
    RunQuery(
      'SELECT * FROM sensors.sensor_definition', 
      [], 
      SensorDefinitionModel.dbResultToSensorDefinitionArray, 
      callback);  
  else
    RunQuery(
      'SELECT * FROM sensors.sensor_definition WHERE id = $1', 
      [query.id], 
      SensorDefinitionModel.dbResultToSensorDefinition, 
      callback);  
}

// Sensor Values
var readSensorValues = function(query, callback){
  if (
    query.id != undefined && 
    query.start != undefined &&
    query.end != undefined &&
    query.max != undefined
    ) {
      RunQuery(
        'SELECT * FROM sensors.get_sensor_data($1, $2, $3, $4)', 
        [query.id, query.start, query.end, query.max], 
        SensorDefinitionModel.dbResultToSensorValueArray, 
        callback);  
  } else if 
    (
    query.id != undefined &&
    query.n != undefined
    ) {
      RunQuery(
        'select * from sensors.sensor_data where sensor_id = $1 and ts <= now() order by ts desc limit $2', 
        [query.id, query.n], 
        SensorDefinitionModel.dbResultToSensorValueArray, 
        callback);  
  } else {
    callback(new Error("Input parameters don't match"), null);
  }
}


var readRelay = function(query, callback){
  if (query.id == undefined) {
    RunQuery(
      'SELECT * FROM relays.relay_data', 
      [], 
      RelayDefinitionModel.dbResultToRelayDefinitionArray, 
      callback);  
  } else {
    RunQuery(
      'SELECT * FROM relays.relay_data WHERE id = $1', 
      [query.id], 
      RelayDefinitionModel.dbResultToRelayDefinitionArray, 
      callback);  
  }
}

var updateRelay = function(query, body, callback) {
  if (query.id == undefined) {
    callback(new Error("Id parameter is required for the update"), null);
  } else if (!RelayDefinitionModel.IsValid(body)) {
    callback(new Error("Request body is not a valid Relay object."), null);
  } else {    
    readRelay(query, (err, res) => {      
      if (res[0].State != body.State) {
        RunQuery(
          'UPDATE relays.relay_data SET name = $1, state = $2, default_state = $3, description = $4, last_status_change_ts = now() WHERE id = $5', 
          [body.Name, body.State, body.DefaultState, body.Description, query.id], 
          null, 
          () => {
            readRelay(query, callback)
          });  
      } else {
        RunQuery(
          'UPDATE relays.relay_data SET name = $1, default_state = $2, description = $3 WHERE id = $4', 
          [body.Name, body.DefaultState, body.Description, query.id], 
          null, 
          () => {
            readRelay(query, callback)
          });
      }
    })    
  }
}

module.exports = {  
  readSensorDefinitions,
  readSensorValues, 
  readRelay,

  updateRelay
}