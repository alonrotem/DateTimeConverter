#!/usr/bin/python

import http.server
import ssl

httpd = http.server.HTTPServer(('192.168.1.192', 4443), http.server.SimpleHTTPRequestHandler)

httpd.socket = ssl.wrap_socket (httpd.socket, certfile='./docs/self-signed/server.pem', server_side=True)

httpd.serve_forever()
