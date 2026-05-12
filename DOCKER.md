# 🐳 WebApiShop Docker Deployment Guide

## Quick Start

### 1. Build and Run with Docker Compose (Recommended)
```bash
# Build and start all services (API + SQL Server + Redis)
docker-compose up --build

# Run in background
docker-compose up -d --build

# Stop all services
docker-compose down

# Stop and remove volumes (clean slate)
docker-compose down -v
```

### 2. Build Docker Image Only
```bash
# Build the WebApiShop image
docker build -t webapishop:latest .

# Run the container (requires external SQL Server and Redis)
docker run -p 5000:8080 \
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Database=WebApiShop216328971;Integrated Security=True;TrustServerCertificate=true;" \
  -e ConnectionStrings__Redis="host.docker.internal:6379,password=Qq123!@#QQQ" \
  webapishop:latest
```

## 🌐 Access Points

After running `docker-compose up`:

- **WebApiShop API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **SQL Server**: localhost:1433 (sa/YourStrong@Passw0rd)
- **Redis**: localhost:6379 (password: Qq123!@#QQQ)
- **Redis Commander**: http://localhost:8081

## 🔧 Configuration

### Environment Variables
The following environment variables can be customized:

```bash
# Database
ConnectionStrings__DefaultConnection=Server=sqlserver;Database=WebApiShop216328971;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;

# Redis
ConnectionStrings__Redis=redis:6379,password=Qq123!@#QQQ
RedisOptions__Configuration=redis:6379,password=Qq123!@#QQQ

# JWT
Jwt__Key=WebApiShop_SuperSecret_JWT_Key_2024!
Jwt__Issuer=WebApiShop
Jwt__Audience=WebApiShopUsers
Jwt__ExpiresInMinutes=60

# ASP.NET Core
ASPNETCORE_ENVIRONMENT=Development
```

### Volume Mounts
- `./logs:/app/logs` - NLog output files
- `sqlserver-data:/var/opt/mssql` - SQL Server database files
- `redis-data:/data` - Redis persistence

## 🗄️ Database Setup

### First Time Setup
```bash
# Start services
docker-compose up -d

# Run EF Core migrations (from host machine)
dotnet ef database update --project Repository --startup-project WebApiShop

# Or connect to SQL Server and run migrations manually
# Server: localhost,1433
# Username: sa
# Password: YourStrong@Passw0rd
```

### Reset Database
```bash
# Stop services and remove volumes
docker-compose down -v

# Start fresh
docker-compose up -d

# Run migrations again
dotnet ef database update --project Repository --startup-project WebApiShop
```

## 🧪 Testing

### Health Check
```bash
# Check if API is running
curl http://localhost:5000/health

# Test API endpoint
curl http://localhost:5000/api/categories
```

### Load Testing
```bash
# Install hey (HTTP load testing tool)
# Windows: choco install hey
# macOS: brew install hey
# Linux: apt-get install hey

# Test API performance
hey -n 1000 -c 10 http://localhost:5000/api/categories
```

## 🚀 Production Deployment

### Build for Production
```bash
# Build production image
docker build -t webapishop:prod --target runtime .

# Tag for registry
docker tag webapishop:prod your-registry.com/webapishop:latest

# Push to registry
docker push your-registry.com/webapishop:latest
```

### Production Environment Variables
```bash
# Use production database
ConnectionStrings__DefaultConnection="Server=prod-sql-server;Database=WebApiShop;User Id=webapi_user;Password=SecurePassword123!;TrustServerCertificate=true;"

# Use production Redis
ConnectionStrings__Redis="prod-redis-cluster:6379,password=ProductionRedisPassword"

# Production JWT key
Jwt__Key="ProductionSuperSecretJWTKey2024WithMoreComplexity!"

# Production environment
ASPNETCORE_ENVIRONMENT=Production
```

## 🔍 Troubleshooting

### Common Issues

**Port conflicts:**
```bash
# Check what's using port 5000
netstat -ano | findstr :5000

# Use different port
docker-compose up --build -p 5001:8080
```

**SQL Server connection issues:**
```bash
# Check SQL Server logs
docker-compose logs sqlserver

# Connect to SQL Server container
docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd
```

**Redis connection issues:**
```bash
# Check Redis logs
docker-compose logs redis

# Connect to Redis container
docker-compose exec redis redis-cli -a Qq123!@#QQQ
```

### Logs
```bash
# View API logs
docker-compose logs webapishop-api

# Follow logs in real-time
docker-compose logs -f webapishop-api

# View all service logs
docker-compose logs
```

## 📊 Monitoring

### Container Stats
```bash
# View resource usage
docker stats

# View specific container
docker stats webapishop_webapishop-api_1
```

### Application Metrics
- **NLog files**: `./logs/` directory
- **Redis metrics**: http://localhost:8081 (Redis Commander)
- **SQL Server**: Connect with SQL Server Management Studio

---

**Happy Dockerizing! 🐳✨**