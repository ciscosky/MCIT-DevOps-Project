# Stage 1: Build the Angular app
FROM node:20 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json and install dependencies
COPY package*.json ./
RUN npm install

# Copy the source code
COPY . .

# Build the app
RUN npm run build -- --configuration production

# Stage 2: Serve the Angular app using NGINX
FROM nginx:alpine

# Copy the built files to the NGINX container
COPY --from=build /app/dist/client-side/browser /usr/share/nginx/html

# Expose port 80
EXPOSE 80


# Replace API_URL in the generated main.*.js file and run NGINX
CMD /bin/sh -c "\
    API_URL=${API_URL} && \
    sed -i 's|API_URL_PLACEHOLDER|'\"$API_URL\"'|g' /usr/share/nginx/html/main-*.js && \
    nginx -g 'daemon off;'"
