.PHONY: build
.NOTPARALLEL:
build: 
	cd src/shared-session && $(MAKE) build-session && $(MAKE) build-web
	cd src/front-end/vatic-share && npm install && npm run build

.PHONY: clean
.NOTPARALLEL:
clean:
	cd src/shared-session && $(MAKE) clean-session && $(MAKE) clean-web
	rm -rf src/front-end/vatic-share/dist

.PHONY: go-fmt
.NOTPARALLEL:
go-fmt:
	cd src/shared-session && $(MAKE) go-fmt

.PHONY: run
.NOTPARALLEL:
run: stop
	docker-compose up --build -d

.PHONY: stop
.NOTPARALLEL:
stop:
	docker-compose down
