var SchemaObject = require('schema-object');

var SensorValueLimits = new SchemaObject({
    CriticalHigh: Number,
    CriticalLow:Number,
    OptimalHigh:Number,
    OptimalLow:Number    
});

var SensorValue = new SchemaObject({
    Date:Object,
    Value:Number
});

var SensorDefinition = new SchemaObject({
    Id:Number,
    SensorName: String,
    SensorValueLimits: SensorValueLimits,
    Unit:String,
    Description: String
});

var dbResultToSensorDefinitionArray = function(result){
    var resArray = [];
    console.log('Rows to array');
    result.rows.forEach(element => {
        resArray.push(new SensorDefinition({
            Id: element.id,
            SensorName: element.name,
            SensorValueLimits: new SensorValueLimits({
                CriticalHigh: element.critical_high,
                CriticalLow: element.critical_low,
                OptimalHigh: element.optimal_high,
                OptimalLow: element.optimal_low
            }),
            Unit: element.measuring_unit,
            Description: element.description
        }));        
    }); 
    return resArray;
}

var dbResultToSensorDefinition = function(element){      
    return new SensorDefinition({
                Id: element.rows[0].id,
                SensorName: element.rows[0].name,
                SensorValueLimits: new SensorValueLimits({
                    CriticalHigh: element.rows[0].critical_high,
                    CriticalLow: element.rows[0].critical_low,
                    OptimalHigh: element.rows[0].optimal_high,
                    OptimalLow: element.rows[0].optimal_low
                }),
                Unit: element.rows[0].measuring_unit,
                Description: element.rows[0].description
            });                    
}

var dbResultToSensorValueArray = function(result){
    var resArray = [];
    result.rows.forEach(element => {
        
        resArray.push(new SensorValue({
            Id: element.id,
            Date: element.ts,
            Value:element.value            
        }));        
    });    
    return resArray;
}

module.exports = {
    SensorValueLimits,
    SensorDefinition,
    SensorValue,
    dbResultToSensorDefinitionArray,
    dbResultToSensorDefinition,
    dbResultToSensorValueArray
};