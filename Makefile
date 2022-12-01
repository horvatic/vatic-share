.PHONY: build
.NOTPARALLEL:
build: clean
	cd session-manager && go mod tidy
	mkdir -p session-manager/build/bin
	go build -o session-manager/build/bin/session-manager session-manager/cmd/session-manager/main.go

	cd file-session && go mod tidy
	mkdir -p file-session/build/bin
	go build -o file-session/build/bin/file-session file-session/cmd/file-session/main.go

.PHONY: clean
.NOTPARALLEL:
clean:
	rm -rf session-manager/build/bin
	rm -rf file-session/build/bin