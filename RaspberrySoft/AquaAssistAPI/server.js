var db = require("./db/data_access.js");
var response = require("./models/Response.js");

var debug = true;

// Require the framework and instantiate it
const api = require('fastify')();

// Routes

api.route({
    method:'GET',
    url:'/Sensor',        
    beforeHandler:async(request, reply) => {
        // TODO Check authentification
    },
    handler:async(request, reply) => GetSensor(request, reply)
})

api.route({
    method:'GET',
    url:'/Sensor/Values',        
    beforeHandler:async(request, reply) => {
        // TODO Check authentification
    },
    handler:async(request, reply) => GetSensorValues(request, reply)
})

api.route({
    method:'GET',
    url:'/Relay',        
    beforeHandler:async(request, reply) => {
        // TODO Check authentification
    },
    handler:async(request, reply) => GetRelay(request, reply)
})

api.route({
    method:'PUT',
    url:'/Relay',        
    beforeHandler:async(request, reply) => {
        // TODO Check authentification
    },
    handler:async(request, reply) => UpdateRelay(request, reply)
})

GetSensor = function(request, reply){
    db.readSensorDefinitions(request.query, (err, res) => {        
        DoReply(reply, err, res);
    });
}

GetSensorValues = function(request, reply){
    db.readSensorValues(request.query, (err, res) => {        
        DoReply(reply, err, res);
    });
}

GetRelay = function(request, reply){
    db.readRelay(request.query, (err, res) => {        
        DoReply(reply, err, res);
    });
}

UpdateRelay = function(request, reply){
    db.updateRelay(request.query, request.body, (err, res) => {        
        DoReply(reply, err, res);
    });
}

DoReply = function(reply, err, res){
    if (err){
        console.log('--------------------ERROR------------------------');
        console.log('Send reply 400: ' + JSON.stringify(err, null, 2));
        reply.code(400).header('Content-Type', 'application/json; charset=utf-8').send(err);    
    } else {
        if (debug){
            console.log('---------------------200-------------------------');
            console.log('Send reply 200: ' + JSON.stringify(res, null, 2));
        }
        reply.code(200).header('Content-Type', 'application/json; charset=utf-8').send(res);
    }
}

// Run the server!
const start = async () => {
  try {
    await api.listen(8080)    
    api.log.info(`server listening on ${api.server.address().port}`)
  } catch (err) {
    api.log.error(err)
    process.exit(1)
  }
}
start()
