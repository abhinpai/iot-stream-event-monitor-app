var app = require('express')();
var http = require('http').createServer(app);
var io = require('socket.io')(http);
var kafka = require('kafka-node');
var Consumer = kafka.Consumer;
var Offset = kafka.Offset;
var Client = kafka.KafkaClient;
var topic = ["bd2a5ef7-1013-4448-93e8-c73253161308_granted",
  "bd2a5ef7-1013-4448-93e8-c73253161308_notgranted",
  "b1738939-268d-41d1-ad67-6ac4cccb4f48_granted",
  "b1738939-268d-41d1-ad67-6ac4cccb4f48_notgranted",
  "ac95dc85-3f0c-4662-b4c4-08759f70ca6a_granted",
  "ac95dc85-3f0c-4662-b4c4-08759f70ca6a_notgranted"];

var client = new Client({ kafkaHost: '13.92.121.134:29092' });
var topics = [{ topic: topic[0], partition: 0 }, { topic: topic[1], partition: 0 }, { topic: topic[2], partition: 0 }, { topic: topic[3], partition: 0 }, { topic: topic[4], partition: 0 }, { topic: topic[5], partition: 0 }];
var options = { autoCommit: false, fetchMaxWaitMs: 1000, fetchMaxBytes: 1024 * 1024 };

var consumer = new Consumer(client, topics, options);
var offset = new Offset(client);

app.get('/', function (req, res) {
});

http.listen(3020, function () {
  console.log('listening on *:3020');
});

consumer.on('message', function (message) {
  console.log(message);
  io.emit('message', message);
});