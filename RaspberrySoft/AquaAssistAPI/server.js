var db = require("./db/data_access.js");
var response = require("./models/Response.js");

// Require the framework and instantiate it
const api = require('fastify')()

// Get all Sensor definition
api.get('/SensorDefinition', async (request, reply) => {      
    if (request.query.id != undefined){
        db.readSensorDefinitionById(request.query.id, (res)=>{
            console.log(res);      
            reply
            .code(200)
            .header('Content-Type', 'application/json; charset=utf-8')
            .send(res);        
        });
    } else {
        db.readSensorDefinitions((res) => {
            console.log(res);                
            reply
                .code(200)
                .header('Content-Type', 'application/json; charset=utf-8')
                .send(res);        
        });    
    } 
})

api.get('/SensorValues', async (request, reply) => {      
    if (request.query.id != undefined){
        db.readSensorValuesBySensorId(request.query.id, (res)=>{
            console.log(res);      
            reply
            .code(200)
            .header('Content-Type', 'application/json; charset=utf-8')
            .send(res);        
        });
    } else {
        db.readSensorDefinitions((res) => {            
            reply
                .code(404)                
                .send("Error");        
        });    
    } 
})

// Run the server!
const start = async () => {
  try {
    await api.listen(3000)
    api.log.info(`server listening on ${api.server.address().port}`)
  } catch (err) {
    api.log.error(err)
    process.exit(1)
  }
}
start()
