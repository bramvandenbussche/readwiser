docker build -t readwiser-api -f bramvandenbussche.readwiser.api/Dockerfile .
docker build -t readwiser-web -f bramvandenbussche.readwiser.web/Dockerfile .
docker compose up -d