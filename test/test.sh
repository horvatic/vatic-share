#! /bin/bash

echo "Testing Hello Data in session 1"
curl "http://localhost:8080/pushdata?data=hellodata&sessionid=1"
echo
echo "Testing fetch in session 1"
curl "http://localhost:8080/readdata?sessionid=1"
echo

echo "Testing Goodbye Data in ssesion 2"
curl "http://localhost:8080/pushdata?data=Goodyedata&sessionid=2"
echo
echo "Testing fetch in session 2"
curl "http://localhost:8080/readdata?sessionid=2"
echo

echo "Testing fetch in session 1"
curl "http://localhost:8080/readdata?sessionid=1"
echo