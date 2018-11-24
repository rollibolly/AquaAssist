var db = require("./db/data_access.js");
var bodyParser = require('body-parser')

var jsonParser = bodyParser.json()

DoReply = function(reply, err, res){    
    if (err){
        console.log('--------------------ERROR------------------------');
        console.log(err);
        reply.send(err);    
    } else {        
        reply.json(res);
    }
}

GetSensor = function(request, reply){    
    console.log('GetSensor')
    db.readSensorDefinitions(request.query, (err, res) => {        
        DoReply(reply, err, res);
    });    
}

GetSensorValues = function(request, reply){
    console.log('GetSensorValues')
    db.readSensorValues(request.query, (err, res) => {        
        DoReply(reply, err, res);
    });
}

GetRelay = function(request, reply){
    console.log('GetRelay')
    db.readRelay(request.query, (err, res) => {        
        DoReply(reply, err, res);
    });
}

UpdateRelay = function(request, reply){   
    console.log('UpdateRelay')
    db.updateRelay(request.query, request.body, (err, res) => {        
        DoReply(reply, err, res);
    });
}

var express = require('express'),
  app = express(),
  port = process.env.PORT || 3000;

  app.route('/Sensor')
  .get(GetSensor)
  app.route('/Sensor/Values')
  .get(GetSensorValues)
  app.route('/Relay')
  .get(GetRelay)
  app.route('/Relay')
  .put(jsonParser, UpdateRelay)

  app.use(bodyParser.json());
  
app.listen(port);

console.log('todo list RESTful API server started on: ' + port);

