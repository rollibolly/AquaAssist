var SchemaObject = require('schema-object');

var RelayDefinition = new SchemaObject({
    Id:Number,
    Name: String,
    State: Boolean,
    DefaultState:Boolean,
    Description: String,
    LastStatusChange: Object
});

var IsValid = function(obj){
    console.log(obj);
    if (obj != undefined &&
        obj.Id != undefined &&
        obj.Name != undefined &&
        obj.State != undefined &&
        obj.DefaultState != undefined &&
        obj.Description != undefined &&
        obj.LastStatusChange != undefined)
        return true;
    return false;
}

var dbResultToRelayDefinitionArray = function(result){
    var resArray = [];    
    result.rows.forEach(element => {
        resArray.push(dbResultToRelayDefinition(element));        
    });    
    return resArray;
}

var dbResultToRelayDefinition = function(element){      
    return new RelayDefinition({
                Id: element.id,
                Name: element.name,                
                State: element.state,
                DefaultState: element.default_state,
                Description: element.description,
                LastStatusChange: element.last_status_change_ts
            });                    
}

var dbResultToRelayDefinitionSingle = function(element){  
    console.log(element)    
    return new RelayDefinition({
                Id: element.rows[0].id,
                Name: element.rows[0].name,                
                State: element.rows[0].state,
                DefaultState: element.rows[0].default_state,
                Description: element.rows[0].description,
                LastStatusChange: element.rows[0].last_status_change_ts
            });                    
}

module.exports = {
    RelayDefinition,
    dbResultToRelayDefinitionArray,
    dbResultToRelayDefinition,
    dbResultToRelayDefinitionSingle,
    IsValid
};