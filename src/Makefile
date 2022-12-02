.PHONY: build
.NOTPARALLEL:
build: clean
	cd session/session-manager && go mod tidy && go build -o build/bin/session-manager cmd/session-manager/main.go
	cd session/file-session && go mod tidy && go build -o build/bin/file-session cmd/file-session/main.go
	cd session/boot-strap && go mod tidy && go build -o build/bin/boot-strap cmd/boot-strap/main.go

.PHONY: clean
.NOTPARALLEL:
clean:
	rm -rf session/session-manager/build/bin
	rm -rf session/file-session/build/bin
	rm -rf session/boot-strap/build/bin