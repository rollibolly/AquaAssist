import serial
import time
from flask import Flask, request
from flask_restful import Resource, Api
from sqlalchemy import create_engine
from json import dumps


################################################################
#                   Initialization                             #
################################################################

app = Flask(__name__)
api = Api(app)

# connection to serial port
ser = serial.Serial()
ser.baudrate = 9600
ser.port = 'COM4'
ser.open()

time.sleep(5) # wait 5 seconds for arduino to reset

msg = ser.readline()
print(msg)

################################################################
#                   Endpoint classes                           #
################################################################

# requests a relay state change
class Relay(Resource):
    def get(self, relay_id, state):
        command = 'R' + relay_id
        if (state == 'ON'):
            command = command + '+'
        elif (state == 'OFF'):
            command = command + '-'
        else:
            return 'ERROR: Incorrect state requested'
            
        ser.write(command.encode('utf-8'))
        time.sleep(1)
        msg = ser.readline()
        return msg.decode('utf-8')

# Request information from the arduino
class Info(Resource):
    def get(self):
        command = 'I';
        ser.write(command.encode('utf-8'))
        time.sleep(1)
        msg = ser.read_all();
        return msg.decode('utf-8')

# Sends a message to display on the aruino LCD
class Message(Resource):
    def get(self, txt):
        command = 'T' + txt
        ser.write(command.encode('utf-8'))
        time.sleep(1)
        msg = ser.readline()
        return msg.decode('utf-8')

# URL mappings
api.add_resource(Relay,   '/relay/<relay_id>/<state>')
api.add_resource(Message, '/message/<txt>')
api.add_resource(Info,    '/info')

# run the REST api
if __name__ == '__main__':
    app.run(port=5002)