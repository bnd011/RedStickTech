import mysql.connector 

def addSchedule(umail, URL, Item):
    mydb = mysql.connector.connect(user='doadmin', password='wwd0oli7w2rplovh', host='rst-db-do-user-8696039-0.b.db.ondigitalocean.com', database='RST_DB', port='25060')
    
    mycursor = mydb.cursor()
    sql = "INSERT INTO schedules (user_email, url, item) values (%s, %s, %s)"
    val = (str(umail), str(URL), str(Item))
    
    mycursor.execute(sql, val)
    mydb.commit()

addSchedule('bro@gmail.com', 'www.bestbuy.com', 'controller')
    


