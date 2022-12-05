.PHONY: build
.NOTPARALLEL:
build: 
	cd src && $(MAKE) build-session && $(MAKE) build-web

.PHONY: clean
.NOTPARALLEL:
clean:
	cd src && $(MAKE) clean-session && $(MAKE) clean-web

.PHONY: go-fmt
.NOTPARALLEL:
go-fmt:
	cd src && $(MAKE) go-fmt

.PHONY: run
.NOTPARALLEL:
run: stop
	docker-compose up --build -d

.PHONY: stop
.NOTPARALLEL:
stop:
	docker-compose down

.PHONY: test
.NOTPARALLEL:
test:
	cd test &&  chmod +x test.sh && ./test.sh