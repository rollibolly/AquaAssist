var SchemaObject = require('schema-object');

var RelayDefinition = new SchemaObject({
    Id:Number,
    Name: String,
    State: Boolean,
    DefaultState:Boolean,
    Description: String
});

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
                Description: element.description
            });                    
}

module.exports = {
    RelayDefinition,
    dbResultToRelayDefinitionArray,
    dbResultToRelayDefinition
};