FROM node:lts-alpine as build
WORKDIR /src
COPY ["package.json", "."]
COPY ["yarn.lock", "."]
RUN yarn install
COPY . .
RUN yarn build

FROM node:lts-alpine as production
COPY --from=build /src/.output /app
WORKDIR /app
EXPOSE 3000
CMD ["node", "server/index.mjs"]