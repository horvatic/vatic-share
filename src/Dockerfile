FROM golang:alpine AS go-builder

RUN apk --no-cache add make

ENV GOOS=linux

WORKDIR /build

COPY . .

RUN make build

FROM ubuntu:latest as final

WORKDIR /app

COPY --from=go-builder /build/session/session-manager/build/bin/session-manager .
COPY --from=go-builder /build/session/file-session/build/bin/file-session .
COPY --from=go-builder /build/session/boot-strap/build/bin/boot-strap .

CMD ["./boot-strap"]
