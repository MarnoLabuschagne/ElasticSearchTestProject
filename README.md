#1  docker pull docker.elastic.co/elasticsearch/elasticsearch:8.6.0

#2  docker network create elastic

#3  docker run --name es01 --net elastic -p 9200:9200 -it docker.elastic.co/elasticsearch elasticsearch:8.6.0
	save the password, http certificate, and the enrollment token


If bootstrap error when trying to run:

#1 wsl -d docker-desktop

#2 sysctl -w vm.max_map_count=262144

#3 exit

    
To make sure it works:

#1  docker cp es01:/usr/share/elasticsearch/config/certs/http_ca.crt .

    if needed: Remove-item alias:curl

#2  curl --cacert http_ca.crt -k -u elastic https://localhost:9200
