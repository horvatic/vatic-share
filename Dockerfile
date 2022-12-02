FROM golang:alpine AS builder

RUN apk --no-cache add make

ENV GOOS=linux

WORKDIR /build

COPY . .

RUN make build

FROM ubuntu:latest as final

WORKDIR /app

COPY --from=builder /build/session-manager/build/bin/session-manager .
COPY --from=builder /build/file-session/build/bin/file-session .
COPY --from=builder /build/boot-strap/build/bin/boot-strap .

CMD ["./boot-strap"]
