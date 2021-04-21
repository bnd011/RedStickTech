import mysql.connector 

def addSchedule(umail, Verify, Salt, EmailVerified):
    mydb = mysql.connector.connect(user='doadmin', password='wwd0oli7w2rplovh', host='rst-db-do-user-8696039-0.b.db.ondigitalocean.com', database='RST_DB', port='25060')
    
    mycursor = mydb.cursor()
    sql = "INSERT INTO user_login_info (user_email, verify, salt, emailVerified) values (%s, %s, %s, %s)"
    val = (str(umail), str(Verify), str(Salt), str(EmailVerified))
    
    mycursor.execute(sql, val)
    mydb.commit()

addSchedule('bigman@gmail.com', 'abcdefghij', 'a;skljfdlksadj', '0')
