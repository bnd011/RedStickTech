import mysql.connector 

def showAdmin():
    mydb = mysql.connector.connect(user='doadmin', password='wwd0oli7w2rplovh', host='rst-db-do-user-8696039-0.b.db.ondigitalocean.com', database='RST_DB', port='25060')
    
    mycursor = mydb.cursor()
    
    
    
    mycursor.execute("SELECT * FROM schedules")
    myresult = mycursor.fetchall()
    a = []
    for x in myresult:
        a.append(x)
    print("Schedule Info:\n", a)

    mycursor.execute("SELECT * FROM user_login_info")
    myresult = mycursor.fetchall()
    b = []
    for y in myresult:
        b.append(y)
    print("User Login Info:\n", b)

showAdmin()
