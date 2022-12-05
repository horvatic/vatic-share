#! /bin/bash

echo "Testing Hello Data in"
curl http://localhost:8080/pushdata?data=hellodata
echo
echo "Testing fetch"
curl http://localhost:8080/readdata
echo