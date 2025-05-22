# CryptoMessenger

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)
![Language](https://img.shields.io/badge/language-C%23-239120.svg)
![Framework](https://img.shields.io/badge/framework-WPF-0078d4.svg)
![Encryption](https://img.shields.io/badge/encryption-AES--256-red.svg)
![Key Exchange](https://img.shields.io/badge/key%20exchange-ECDH-green.svg)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)

A Windows Presentation Foundation (WPF) application demonstrating secure communication principles including **Elliptic Curve Diffie-Hellman (ECDH) key exchange** and **AES encryption** for real-time encrypted messaging.

## 🔐 Features

- **End-to-End Encryption**: Messages are encrypted using AES-256 with CBC mode
- **Secure Key Exchange**: Implements Elliptic Curve Diffie-Hellman (ECDH) for secure key generation
- **Real-time Communication**: TCP-based client-server architecture
- **User-Friendly Interface**: Modern WPF interface with separate client and server windows
- **Connection Logging**: Detailed logs showing encryption/decryption process
- **Secure Channel Establishment**: Visual confirmation when secure communication is ready

## 🛡️ Security Implementation

### Cryptographic Protocols Used

1. **Elliptic Curve Diffie-Hellman (ECDH)**
   - Key exchange protocol for establishing shared secrets
   - Uses SHA-256 for key derivation
   - Provides forward secrecy

2. **AES Encryption**
   - Advanced Encryption Standard with 256-bit keys
   - CBC (Cipher Block Chaining) mode
   - PKCS7 padding
   - Unique initialization vectors (IV) for each session

3. **Secure Communication Flow**
   ```
   1. Server generates ECDH key pair and sends public key
   2. Server generates and sends AES initialization vector (IV)
   3. Client receives server's public key and generates own key pair
   4. Client sends its public key to server
   5. Both parties derive shared secret using ECDH
   6. Secure channel established - all messages encrypted with AES
   ```

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 or later
- Windows operating system
- Visual Studio 2022 (recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/secure-communication-demo.git
   cd secure-communication-demo
   ```

2. **Open the solution**
   ```bash
   # Open in Visual Studio
   start Secure_communication.sln
   
   # Or build from command line
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

## 📖 Usage

### Starting the Application

1. **Launch the main window** - Choose between Client or Server mode
2. **Server Setup**:
   - Click "Start Server" 
   - Server will listen on `127.0.0.1:13000` by default
   - Wait for client connections
3. **Client Setup**:
   - Click "Start Client"
   - Enter server IP address (default: `127.0.0.1`)
   - Click "Connect"

### Secure Communication Process

1. **Key Exchange Phase** (Automatic):
   - Server initiates ECDH key exchange
   - Client responds with its public key
   - Both parties derive shared secret
   - Server sends AES initialization vector

2. **Secure Messaging**:
   - Type messages in the message box
   - Click "Send" to transmit encrypted messages
   - View encryption/decryption logs in real-time

### Example Communication Flow

```
Server Log:
[INFO] Waiting for a connection...
[INFO] Connected
[INFO] Initiating Key exchange...
[INFO] Public key Sent...
[INFO] Initiating IV exchange...
[INFO] IV Sent...
[INFO] Public key Received...
[INFO] Secret key Generated for secure Communication...
[SUCCESS] >>>>>>>>>>Secure Channel Established<<<<<<<<<<

Client Log:
[INFO] Connected to: 127.0.0.1
[INFO] Initiating Key exchange...
[INFO] Received: Public key of server...
[INFO] Public key Sent...
[INFO] Generating Secret key for secure Communication...
[INFO] Secret key Generated for secure Communication...
[SUCCESS] >>>>>>>>>>Secure Channel Established<<<<<<<<<<
```

## 🏗️ Architecture

### Project Structure

```
├── App.xaml                    # Application entry point
├── MainWindow.xaml             # Main selection window
├── Client_window.xaml          # Client interface
├── Server_window.xaml          # Server interface
└── Service/
    ├── AES_Service.cs          # AES encryption/decryption
    ├── KeyExchange_Service.cs  # ECDH key exchange
    ├── Server_Service.cs       # Server networking logic
    └── client_Service.cs       # Client networking logic
```

### Key Components

- **AES_Service**: Handles AES encryption and decryption operations
- **KeyExchange_Service**: Manages ECDH key generation and shared secret derivation
- **Server_Service**: TCP server implementation with encryption integration
- **client_Service**: TCP client implementation with encryption integration

## 🔧 Configuration

### Network Settings

- **Default Port**: `13000`
- **Default IP**: `127.0.0.1` (localhost)
- **Protocol**: TCP

### Security Settings

- **AES Key Size**: 256 bits (derived from ECDH shared secret)
- **AES Mode**: CBC (Cipher Block Chaining)
- **Padding**: PKCS7
- **Hash Algorithm**: SHA-256
- **Elliptic Curve**: Default system curve (typically P-256)

## 🛠️ Development

### Building from Source

```bash
# Clone the repository
git clone https://github.com/yourusername/secure-communication-demo.git

# Navigate to project directory
cd secure-communication-demo

# Restore dependencies
dotnet restore

# Build the project
dotnet build --configuration Release

# Run the application
dotnet run
```

### Testing

1. **Single Machine Testing**:
   - Run the application
   - Start server first, then client
   - Use default localhost settings

2. **Network Testing**:
   - Run server on one machine
   - Update client IP address to server's network IP
   - Ensure port 13000 is accessible

## 📋 Requirements

- **Framework**: .NET 8.0
- **Platform**: Windows (WPF requirement)
- **Libraries**: 
  - System.Security.Cryptography
  - System.Net.Sockets
  - Windows Presentation Foundation

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## ⚠️ Security Considerations

- **Educational Purpose**: This application is designed for educational demonstration
- **Production Use**: Additional security measures recommended for production environments
- **Key Management**: Consider implementing proper key storage and rotation
- **Authentication**: Current implementation focuses on encryption; authentication not included
- **Network Security**: Use with secure networks; consider adding TLS layer for additional protection

## 🐛 Known Issues

- Application designed for single client-server pair
- No persistent key storage
- Basic error handling implementation
- Windows-only due to WPF dependency


---

**Note**: This application demonstrates cryptographic concepts for educational purposes. For production applications, consider using established secure communication protocols like TLS/SSL.
