FROM --platform=linux/amd64 golang:alpine AS go-builder
RUN apk --no-cache add make
ENV GOOS=linux
WORKDIR /build
COPY ./session ./session
COPY Makefile Makefile
RUN make build-session

FROM --platform=linux/amd64 ubuntu:latest  AS dotnet-builder
RUN apt-get update && apt-get install -y make curl dotnet-sdk-6.0
WORKDIR /build
COPY ./web-interface ./web-interface
COPY Makefile Makefile
RUN make build-web

FROM --platform=linux/amd64 ubuntu:latest as final
ENV ASPNETCORE_hostBuilder__reloadConfigOnChange=false
RUN apt-get update && apt-get install -y apt-transport-https libicu-dev
WORKDIR /app
COPY --from=go-builder /build/session/session-manager/build/bin/session-manager .
COPY --from=go-builder /build/session/file-session/build/bin/file-session .
COPY --from=go-builder /build/session/boot-strap/build/bin/boot-strap .
COPY --from=dotnet-builder /build/web-interface/build/bin/Share.Web .
COPY --from=dotnet-builder /build/web-interface/build/bin/appsettings.json .
CMD ["./boot-strap"]
