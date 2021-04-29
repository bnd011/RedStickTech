from mysql.connector import (connection)

def Connect():
    cnx = connection.MySQLConnection(user='doadmin', password='wwd0oli7w2rplovh', host='rst-db-do-user-8696039-0.b.db.ondigitalocean.com', database='RST_DB', port='25060')

    mycursor = cnx.cursor()

    mycursor.execute("SELECT * FROM schedules")
    myresult = mycursor.fetchall()
    a = []
    for x in myresult:
        a.append(x)
    return a

Connect()
