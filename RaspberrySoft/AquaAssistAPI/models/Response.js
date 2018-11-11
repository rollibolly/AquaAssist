var SchemaObject = require('schema-object');

var ResponseMessage = new SchemaObject({
    Success:Boolean,
    Message:String,
    Data:String
});

module.exports = {
    ResponseMessage
}