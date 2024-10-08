const { createServer } = require('node:http');

const hostname = 'localhost';//=locallhost
const port = 2402;

const server = createServer((req, res) => {
  res.statusCode = 200;
  res.setHeader('Content-Type', 'text/plain');
  res.end('Hello Phuc Binh dep chai nhat the gioi');
});

server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});
