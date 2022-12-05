.PHONY: all
.NOTPARALLEL:
all: build-session build-web

.PHONY: build-session
.NOTPARALLEL:
build-session: clean-session
	cd session/session-manager && go mod tidy && go build -o build/bin/session-manager cmd/session-manager/main.go
	cd session/file-session && go mod tidy && go build -o build/bin/file-session cmd/file-session/main.go
	cd session/boot-strap && go mod tidy && go build -o build/bin/boot-strap cmd/boot-strap/main.go

.PHONY: build-web
.NOTPARALLEL:
build-web: clean-web
	cd web-interface && dotnet publish --output "build/bin" --runtime linux-x64 --configuration Release -p:PublishSingleFile=true --self-contained true

.PHONY: clean-session
.NOTPARALLEL:
clean-session:
	rm -rf session/session-manager/build/bin
	rm -rf session/file-session/build/bin
	rm -rf session/boot-strap/build/bin

.PHONY: clean-web
.NOTPARALLEL:
clean-web:
	rm -rf web-interface/build/bin
	rm -rf web-interface/bin
	rm -rf web-interface/obj

.PHONY: go-fmt
.NOTPARALLEL:
go-fmt:
	cd session/session-manager && go fmt ./...
	cd session/file-session && go fmt ./...
	cd session/boot-strap && go fmt ./...
	cd session/shared-constants && go fmt ./...

.PHONY: run
.NOTPARALLEL:
run: clean-session clean-web
	docker-compose up --build -d

.PHONY: stop
.NOTPARALLEL:
stop:
	docker-compose down