.PHONY: build
.NOTPARALLEL:
build: clean
	cd session-manager && go mod tidy && go build -o build/bin/session-manager cmd/session-manager/main.go
	cd file-session && go mod tidy && go build -o build/bin/file-session cmd/file-session/main.go
	cd boot-strap && go mod tidy && go build -o build/bin/boot-strap cmd/boot-strap/main.go

.PHONY: clean
.NOTPARALLEL:
clean:
	rm -rf session-manager/build/bin
	rm -rf file-session/build/bin
	rm -rf boot-strap/build/bin