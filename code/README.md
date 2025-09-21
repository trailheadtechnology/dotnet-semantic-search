# .NET Semantic Search Demo

A .NET application demonstrating semantic search capabilities using AI embeddings with Qdrant vector database and Ollama for local AI model inference.

## Features

- 📚 **Blog Post Processing**: Retrieves blog posts from RSS feeds and generates embeddings
- 🔍 **Semantic Search**: Search through blog posts using natural language queries
- 🎯 **Vector Similarity**: Uses AI embeddings to find semantically similar content
- 🐳 **Hybrid Setup**: Qdrant runs in Docker, Ollama runs locally for optimal performance

## Architecture

- **Vector Database**: Qdrant (running in Docker)
- **AI Embeddings**: Ollama with `nomic-embed-text` model (local installation)
- **Framework**: .NET 9
- **AI Integration**: Microsoft.Extensions.AI

## Prerequisites

### Required
- [Docker](https://docs.docker.com/get-docker/) installed and running
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Platform-Specific Tools
- **macOS**: [Homebrew](https://brew.sh/) (recommended for Ollama installation)
- **Windows**: [Chocolatey](https://chocolatey.org/) (optional, for package management)
- **Linux**: `curl` for Ollama installation script

### Verify Prerequisites
```bash
# Check Docker
docker --version

# Check .NET
dotnet --version

# Should show .NET 9.0 or later
```

## Setup Instructions

### 1. Clone and Build the Project

```bash
# Clone the repository
git clone <repository-url>
cd dotnet-semantic-search/code

# Build the project
dotnet build
```

### 2. Set Up Qdrant (Vector Database)

Qdrant runs in Docker for easy setup and management:

```bash
# Start Qdrant using Docker Compose
docker compose up -d

# Verify Qdrant is running
docker ps
```

Qdrant will be available at:
- **HTTP API**: http://localhost:6333
- **Web UI**: http://localhost:6333/dashboard
- **gRPC**: localhost:6334

### 3. Set Up Ollama (AI Model)

Ollama runs locally for better performance. Choose the installation method for your platform:

#### macOS Installation

```bash
# Install Ollama via Homebrew
brew install ollama

# Start Ollama as a service
brew services start ollama

# Pull the embedding model
ollama pull nomic-embed-text

# Verify the model is available
ollama list
```

#### Windows Installation

**Option 1: Direct Download (Recommended)**
1. Download the Ollama installer from [https://ollama.com/download](https://ollama.com/download)
2. Run the installer and follow the setup wizard
3. Ollama will start automatically as a Windows service

**Option 2: Using Chocolatey**
```powershell
# Install Chocolatey first if you haven't already
# Then install Ollama
choco install ollama

# Ollama starts automatically as a service
```

**After Installation (Both Options)**
```powershell
# Pull the embedding model
ollama pull nomic-embed-text

# Verify the model is available
ollama list
```

#### Linux Installation

```bash
# Install Ollama
curl -fsSL https://ollama.com/install.sh | sh

# Start Ollama service
sudo systemctl start ollama
sudo systemctl enable ollama

# Pull the embedding model
ollama pull nomic-embed-text

# Verify the model is available
ollama list
```

Ollama will be available at:
- **API**: http://localhost:11434

### 4. Run the Application

```bash
# Navigate to the project directory
cd dotnet-semantic-search

# Run the application
dotnet run
```

## Usage

The application provides a simple menu interface:

### 1. 📚 Process Blog Posts
- Retrieves blog posts from configured RSS feeds
- Generates AI embeddings for each post
- Stores embeddings in Qdrant vector database
- **First time setup**: Choose this option to populate the database

### 2. 🔍 Search Blog Posts
- Enter natural language search queries
- Uses AI embeddings to find semantically similar content
- Returns ranked results with similarity scores
- **Example queries**: "API security", "React performance", "Azure deployment"

### 3. 🚪 Exit
- Cleanly exits the application

## Configuration

### RSS Feed Source
The application is configured to process blog posts from Trailhead Technology's RSS feed. You can modify the RSS source in the code if needed.

### Model Configuration
- **Embedding Model**: `nomic-embed-text` (274 MB)
- **Vector Dimensions**: 768
- **Embedding Provider**: Ollama

## Troubleshooting

### Qdrant Issues

```bash
# Check if Qdrant container is running
docker ps

# View Qdrant logs
docker compose logs qdrant

# Restart Qdrant
docker compose restart qdrant
```

### Ollama Issues

#### macOS
```bash
# Check if Ollama service is running
brew services list | grep ollama

# Restart Ollama service
brew services restart ollama

# Check available models
ollama list

# Re-pull the model if needed
ollama pull nomic-embed-text
```

#### Windows
```powershell
# Check if Ollama service is running
Get-Service -Name "Ollama*"

# Restart Ollama service (run as Administrator)
Restart-Service -Name "Ollama*"

# Check available models
ollama list

# Re-pull the model if needed
ollama pull nomic-embed-text
```

#### Linux
```bash
# Check if Ollama service is running
sudo systemctl status ollama

# Restart Ollama service
sudo systemctl restart ollama

# Check available models
ollama list

# Re-pull the model if needed
ollama pull nomic-embed-text
```

### Application Issues

```bash
# Check .NET version
dotnet --version

# Restore packages
dotnet restore

# Clean and rebuild
dotnet clean && dotnet build
```

## Performance Notes

- **Qdrant in Docker**: Provides consistent performance and easy management
- **Ollama Local**: Running Ollama locally (not in Docker) provides significantly better performance for AI model inference
- **First Run**: Initial blog post processing may take several minutes depending on the number of posts
- **Embeddings**: Generated once and cached in Qdrant for fast retrieval

## Technical Details

### Dependencies
- `Microsoft.Extensions.AI` - AI abstraction layer
- `Microsoft.Extensions.AI.Ollama` - Ollama integration
- `Spectre.Console` - Enhanced console output
- Custom services for Qdrant integration and blog retrieval

### Vector Database Schema
- **Collection**: Blog posts with metadata
- **Vectors**: 768-dimensional embeddings
- **Metadata**: Title, content, URL, publish date

### Search Algorithm
1. Convert search query to embedding using Ollama
2. Perform vector similarity search in Qdrant
3. Return top-k results with similarity scores
4. Display results with metadata

## Development

### Project Structure
```
dotnet-semantic-search/
├── Program.cs              # Main application entry point
├── Models/                 # Data models
│   ├── BlogPost.cs        # Blog post model
│   └── EmbeddingDocument.cs # Vector document model
├── Services/              # Business logic
│   ├── QdrantService.cs   # Vector database operations
│   └── BlogRetrievalService.cs # RSS feed processing
└── Utils/                 # Utilities
    ├── ConsoleHelper.cs   # Console UI helpers
    └── Statics.cs         # Constants
```

### Adding New Features
- Extend `QdrantService` for new vector operations
- Modify `BlogRetrievalService` for different content sources
- Update models for additional metadata fields

## License

This project is for demonstration purposes. Check individual dependencies for their respective licenses.