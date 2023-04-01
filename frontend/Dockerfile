# set the base image to the LTS version of node.js for Alpine Linux
FROM node:lts-alpine as builder 

# set the working directory to /app
WORKDIR /app

#copy the current directory into the container at /app
COPY . .    

# install dependencies
RUN npm install \
    --prefer-offline \
    --frozen-lockfile \
    --non-interactive \
    --production=false
    # false because of a linter

#build the app
RUN npm run build

# remove dev dependencies
RUN rm -rf node_modules && \
    NODE_ENV=production npm install \
    --prefer-offline \
    --pure-lockfile \
    --non-interactive \
    --production=false

FROM node:lts-alpine

WORKDIR /app

COPY --from=builder /app .

ENV HOST 0.0.0.0
EXPOSE 80
CMD ["npm","run","start"]




