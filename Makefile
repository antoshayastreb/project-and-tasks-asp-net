up_service:
	docker compose -f ./service.compose.yaml up -d --build
down_service:
	docker compose -f ./service.compose.yaml down
up_db:
	docker compose -f ./service.compose.yaml up -d db