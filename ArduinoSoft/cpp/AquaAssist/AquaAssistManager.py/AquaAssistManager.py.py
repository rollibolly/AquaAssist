import psycopg2
from Schedule import Schedule

def printDebug(row):
    dev_id = row[0];
    name = row[1];
    type = row[2];
    start = row[3];
    end = row[4];
    period = row[5];
    data = row[6];
    last_run = row[7];
    print("--------------------------------")
    print("Device ID: {}".format(dev_id ));
    print("Device Name: {}".format(name));
    print("Device Type: {}".format(type));
    print("Start: {}".format(start));
    print("End: {}".format(end));
    print("Period: {}".format(period));
    print("Data: {}".format(data));
    print("Last Run: {}".format(last_run));

conn = psycopg2.connect("dbname='aqua_assist' user='pi' host='192.168.1.195' port=5432 password='raspberry'")
sch = Schedule(conn);
for s in sch.readAll():
    s.debugPrint();
