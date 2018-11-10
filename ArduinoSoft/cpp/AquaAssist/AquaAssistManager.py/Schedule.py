import psycopg2

class Schedule(object):
    """description of class"""
    
    def __init__(self, conn, row = None):
        self.conn = conn;   
        if (row != None):
            self.dev_id = row[0];
            self.name = row[1];
            self.type = row[2];
            self.start = row[3];
            self.end = row[4];
            self.period = row[5];
            self.data = row[6];
            self.last_run = row[7];

    def readAll(self):
        cur = self.conn.cursor();
        cur.execute("""select dev.dev_id, dev.display_name, dev.dev_type, sch.sched_type, sch.start_time, sch.end_time, sch.period, sch.data, sch.last_run from schedule.devices dev, schedule.schedules sch where dev.dev_id = sch.dev_id""");
        rows = cur.fetchall();

        result = [];
        for row in rows:
            result.append(Schedule(self.conn, row));
        return result;
            


    def debugPrint(self):
        print("--------------------------------")
        print("Device ID: {}".format(self.dev_id ));
        print("Device Name: {}".format(self.name));
        print("Device Type: {}".format(self.type));
        print("Start: {}".format(self.start));
        print("End: {}".format(self.end));
        print("Period: {}".format(self.period));
        print("Data: {}".format(self.data));
        print("Last Run: {}".format(self.last_run));

