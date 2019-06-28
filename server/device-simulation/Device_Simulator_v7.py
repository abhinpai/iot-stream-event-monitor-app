import datetime
import random
import sys
import pika
import os
import uuid

#DeviceId = ['bd2a5ef7-1013-4448-93e8-c73253161308', 
#            'b1738939-268d-41d1-ad67-6ac4cccb4f48',
#            'ac95dc85-3f0c-4662-b4c4-08759f70ca6a',
#            '001738f3-2203-40b1-9f13-b7a885eb7718',
#            '04b0b260-5e38-4b87-9f87-8ccd8c192062']



#deviceId = 'bd2a5ef7-1013-4448-93e8-c73253161308'  

def format_bodyproperties(bodyProperties):
    BodyProperties = []
    for key in bodyProperties.keys():
        tempDict = {}
#        tempKey = bodyProperties[key].values()
        tempValue = bodyProperties[key]
        tempDict["Key"] = key
        tempDict["Value"] = tempValue
#        print(tempDict)
        BodyProperties.append(tempDict)
    
    return(BodyProperties)

def SendJSONData(numberOfEventsToBeCreated = 10, accessType = "", deviceId = ""):
    
#    file = open("C:\\Innovation_Hackathon\\JSONDump.txt", "a")
    connection = pika.BlockingConnection(pika.ConnectionParameters('13.92.121.134'))
    channel = connection.channel()
    
#    file.write(str(numberOfEventsToBeCreated) + " .. " + str(accessType) + " .. " + str(deviceId) + "\n")
#    print(str(numberOfEventsToBeCreated), " .. ", str(accessType), " .. ", str(deviceId))
    
    if(accessType == ""):
    
        for i in range(int(numberOfEventsToBeCreated)):
            
            CreatedTime = str(datetime.datetime.now())
#            EventId = random.randint(1,100)
            EventId = str(uuid.uuid4())
            bodyPropertiesDefinition = {'IsGranted': random.sample(['True', 'False'],1)[0], 
                                        'EmployeeId': str(random.randint(1,100)), 
#                                        'Room': str(random.randint(1,10))
                                        'Room': '10'
                                        }
            
            BodyProperties = format_bodyproperties(bodyPropertiesDefinition)
            
#            deviceId = random.sample(['bd2a5ef7-1013-4448-93e8-c73253161308', 
#            'b1738939-268d-41d1-ad67-6ac4cccb4f48',
#            'ac95dc85-3f0c-4662-b4c4-08759f70ca6a',
#            '001738f3-2203-40b1-9f13-b7a885eb7718',
#            '04b0b260-5e38-4b87-9f87-8ccd8c192062'],1)[0]
            
            JSONStructure = {
                                'CreatedTime': CreatedTime,
                                'EventId': EventId,
#                                'Body': [],
                                'BodyProperties': BodyProperties,
                                'EventType': 'Swipe'
                            }
    
#            file.write(str(JSONStructure))
#            file.write("\n\n")
            print(JSONStructure)
            
            byteArrayToBeSent = str(JSONStructure).encode('utf-8')
            channel.basic_publish(exchange='',
                          routing_key=deviceId,
                          body=byteArrayToBeSent)
            
    elif accessType in ['True', 'False']:
            
        CreatedTime = str(datetime.datetime.now())
#        EventId = random.randint(1,100)
        EventId = str(uuid.uuid4())
        bodyPropertiesDefinition = {'IsGranted': accessType, 
                                    'EmployeeId': str(random.randint(1,100)), 
#                                    'Room': str(random.randint(1,10))
                                    'Room': '10'
                                    }
        
        BodyProperties = format_bodyproperties(bodyPropertiesDefinition)
        
        JSONStructure = {
                            'CreatedTime': CreatedTime,
                            'EventId': EventId,
#                            'Body': [],
                            'BodyProperties': BodyProperties,
                            'EventType': 'Swipe'
                        }

#        file.write(str(JSONStructure))
#        file.write("\n\n")
        print(JSONStructure)
        
        byteArrayToBeSent = str(JSONStructure).encode('utf-8')
        channel.basic_publish(exchange='',
                      routing_key=deviceId,
                      body=byteArrayToBeSent)
        
    
    else:
        print("'Incorrect Access Grant...'")
#        file.write(str('Incorrect Access Grant...'))
    
    connection.close()
#    file.close()
    


#SendJSONData(deviceId, numberOfEventsToBeCreated = 5, accessType = 'False')
#SendJSONData(deviceId, numberOfEventsToBeCreated = 5)
    
#SendJSONData(numberOfEventsToBeCreated = 2, accessType = "", deviceId = 'bd2a5ef7-1013-4448-93e8-c73253161308')

if __name__ == "__main__":
    
    numberOfEventsToBeCreated = float(sys.argv[1])
    accessType = str(sys.argv[2])
    
#    file = open("C:\\Innovation_Hackathon\\config.txt", "r")
#    file = open(os.path.join(sys.path[0], "config.txt"), "r")
    file = open(os.path.realpath(os.path.join(os.getcwd(), os.path.dirname(__file__))) + "\\config.txt")
    deviceId = file.read()
    
#    print(str(numberOfEventsToBeCreated), " ... ", str(accessType), " ... ", str(deviceId))
    
    SendJSONData(numberOfEventsToBeCreated = numberOfEventsToBeCreated, accessType = accessType, deviceId = deviceId)
        
    
    
    
    
