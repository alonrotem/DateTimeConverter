#!/usr/bin/python

# -----------------------------------------------------------------------------------------
# Run HTTP Server:
# ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'; python3 -m http.server 8000
# -----------------------------------------------------------------------------------------
# 
# -----------------------------------------------------------------------------------------
# Run HTTPS Server with self-signed certificate:
# https://pankajmalhotra.com/Simple-HTTPS-Server-In-Python-Using-Self-Signed-Certs
# 
# openssl genrsa -des3 -out server.key 2048
# (set password)
#
# openssl req -new -key server.key -out server.csr
# (enger password, and note the Common Name (e.g. server FQDN or YOUR name) []:<ip address / localhost>)
# 
# openssl x509 -req -days 1024 -in server.csr -signkey server.key -out server.crt
# 
# cat server.crt server.key > server.pem
# 
# Set in the file below: IP address, port, and relative location of the .pem certificate
# -----------------------------------------------------------------------------------------

import http.server
import ssl

httpd = http.server.HTTPServer(('192.168.1.192', 4443), http.server.SimpleHTTPRequestHandler)

httpd.socket = ssl.wrap_socket (httpd.socket, certfile='./docs/self-signed/server.pem', server_side=True)

httpd.serve_forever()
