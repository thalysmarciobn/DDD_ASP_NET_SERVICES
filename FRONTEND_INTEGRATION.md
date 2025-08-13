# Integração Frontend com Códigos de Erro

Este documento explica como o frontend deve integrar com a API usando o sistema de códigos de erro padronizados.

## Estrutura de Resposta

### Resposta de Sucesso
```json
{
  "successCode": 2001,
  "data": {
    "userId": "123e4567-e89b-12d3-a456-426614174000"
  }
}
```

### Resposta de Erro
```json
{
  "errorCode": 1002,
  "errorMessage": "User already exists"
}
```

## Códigos de Erro e Sucesso

### Códigos de Erro (1000-1999)
| Código | Enum | Descrição |
|--------|------|-----------|
| 1001 | `UserNotFound` | Usuário não encontrado |
| 1002 | `UserAlreadyExists` | Usuário já existe |
| 1003 | `InvalidCredentials` | Credenciais inválidas |
| 1004 | `UserInactive` | Usuário inativo |
| 1005 | `InvalidInput` | Dados de entrada inválidos |
| 1006 | `DatabaseError` | Erro de banco de dados |
| 1007 | `ValidationError` | Erro de validação |

### Códigos de Sucesso (2000-2999)
| Código | Enum | Descrição |
|--------|------|-----------|
| 2001 | `UserRegistered` | Usuário registrado com sucesso |
| 2002 | `UserLoggedIn` | Usuário logado com sucesso |
| 2003 | `UserRetrieved` | Usuário recuperado com sucesso |
| 2004 | `OperationCompleted` | Operação concluída com sucesso |

## Endpoints da API

### 1. Registro de Usuário
```
POST /api/auth/register
Content-Type: application/json

{
  "username": "usuario",
  "email": "usuario@email.com",
  "password": "senha123"
}
```

### 2. Login
```
POST /api/auth/login
Content-Type: application/json

{
  "username": "usuario",
  "password": "senha123"
}
```

### 3. Obter Dados do Usuário Logado
```
GET /api/auth/me
Authorization: Bearer {token}
```

## Implementação no Frontend

### TypeScript/JavaScript

#### Definição dos Tipos
```typescript
enum AuthErrorCode {
  None = 0,
  UserNotFound = 1001,
  UserAlreadyExists = 1002,
  InvalidCredentials = 1003,
  UserInactive = 1004,
  InvalidInput = 1005,
  DatabaseError = 1006,
  ValidationError = 1007
}

enum SuccessCode {
  None = 0,
  UserRegistered = 2001,
  UserLoggedIn = 2002,
  UserRetrieved = 2003,
  OperationCompleted = 2004
}

interface RegisterUserData {
  username: string;
  email: string;
  password: string;
}

interface LoginUserData {
  username: string;
  password: string;
}

interface UserData {
  id: string;
  username: string;
  email: string;
  createdAt: string;
  lastLoginAt?: string;
  isActive: boolean;
}

interface ApiResponse<T> {
  successCode?: SuccessCode;
  errorCode?: AuthErrorCode;
  errorMessage?: string;
  data?: T;
}
```

#### Função de Tratamento de Erro
```typescript
function handleApiError(errorCode: AuthErrorCode): string {
  const errorMessages: Record<AuthErrorCode, string> = {
    [AuthErrorCode.None]: 'Sem erro',
    [AuthErrorCode.UserNotFound]: 'Usuário não encontrado',
    [AuthErrorCode.UserAlreadyExists]: 'Usuário já existe',
    [AuthErrorCode.InvalidCredentials]: 'Credenciais inválidas',
    [AuthErrorCode.UserInactive]: 'Usuário inativo',
    [AuthErrorCode.InvalidInput]: 'Dados de entrada inválidos',
    [AuthErrorCode.DatabaseError]: 'Erro de banco de dados',
    [AuthErrorCode.ValidationError]: 'Erro de validação'
  };

  return errorMessages[errorCode] || 'Erro desconhecido';
}
```

#### Funções de API
```typescript
// Registro de usuário
async function registerUser(userData: RegisterUserData): Promise<ApiResponse<{ userId: string }>> {
  const response = await fetch('/api/auth/register', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(userData)
  });

  return await response.json();
}

// Login
async function loginUser(userData: LoginUserData): Promise<ApiResponse<{ token: string; username: string; email: string }>> {
  const response = await fetch('/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(userData)
  });

  return await response.json();
}

// Obter dados do usuário logado
async function getCurrentUser(token: string): Promise<ApiResponse<UserData>> {
  const response = await fetch('/api/auth/me', {
    method: 'GET',
    headers: { 
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    }
  });

  return await response.json();
}
```

#### Exemplo de Uso
```typescript
async function handleLogin(username: string, password: string): Promise<void> {
  try {
    const result = await loginUser({ username, password });

    if (result.successCode) {
      // Sucesso
      const token = result.data!.token;
      localStorage.setItem('authToken', token);
      
      showSuccessMessage('Login realizado com sucesso!');
      redirectToDashboard();
    } else {
      // Erro
      const errorMessage = handleApiError(result.errorCode!);
      showErrorMessage(errorMessage);
    }
  } catch (error) {
    showErrorMessage('Erro de conexão com o servidor');
  }
}

async function loadUserProfile(): Promise<void> {
  const token = localStorage.getItem('authToken');
  if (!token) {
    redirectToLogin();
    return;
  }

  try {
    const result = await getCurrentUser(token);

    if (result.successCode) {
      // Sucesso
      const userData = result.data!;
      displayUserProfile(userData);
    } else {
      // Erro
      if (result.errorCode === AuthErrorCode.InvalidCredentials) {
        localStorage.removeItem('authToken');
        redirectToLogin();
      } else {
        const errorMessage = handleApiError(result.errorCode!);
        showErrorMessage(errorMessage);
      }
    }
  } catch (error) {
    showErrorMessage('Erro de conexão com o servidor');
  }
}
```

### React Hook

```typescript
import { useState, useCallback } from 'react';

interface UseApiResponse<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
  execute: (...args: any[]) => Promise<void>;
}

function useApi<T>(apiCall: (...args: any[]) => Promise<ApiResponse<T>>): UseApiResponse<T> {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const execute = useCallback(async (...args: any[]) => {
    setLoading(true);
    setError(null);
    
    try {
      const result = await apiCall(...args);
      
      if (result.successCode) {
        setData(result.data!);
        setError(null);
      } else {
        const errorMessage = handleApiError(result.errorCode!);
        setError(errorMessage);
        setData(null);
      }
    } catch (err) {
      setError('Erro de conexão com o servidor');
      setData(null);
    } finally {
      setLoading(false);
    }
  }, [apiCall]);

  return { data, loading, error, execute };
}
```

### Exemplo de Componente React

```typescript
function UserProfile() {
  const [token] = useState(localStorage.getItem('authToken'));
  const { data: user, loading, error, execute } = useApi(getCurrentUser);

  useEffect(() => {
    if (token) {
      execute(token);
    }
  }, [token, execute]);

  if (!token) {
    return <Navigate to="/login" />;
  }

  if (loading) {
    return <div>Carregando perfil...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  if (!user) {
    return <div>Nenhum usuário encontrado</div>;
  }

  return (
    <div className="user-profile">
      <h2>Perfil do Usuário</h2>
      <div className="profile-info">
        <p><strong>Username:</strong> {user.username}</p>
        <p><strong>Email:</strong> {user.email}</p>
        <p><strong>Membro desde:</strong> {new Date(user.createdAt).toLocaleDateString()}</p>
        {user.lastLoginAt && (
          <p><strong>Último login:</strong> {new Date(user.lastLoginAt).toLocaleDateString()}</p>
        )}
        <p><strong>Status:</strong> {user.isActive ? 'Ativo' : 'Inativo'}</p>
      </div>
    </div>
  );
}

function LoginForm() {
  const { data, loading, error, execute } = useApi(loginUser);
  const [formData, setFormData] = useState({
    username: '',
    password: ''
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await execute(formData);
  };

  useEffect(() => {
    if (data?.token) {
      localStorage.setItem('authToken', data.token);
      redirectToDashboard();
    }
  }, [data]);

  return (
    <form onSubmit={handleSubmit}>
      {error && <div className="error">{error}</div>}
      
      <input
        type="text"
        placeholder="Username"
        value={formData.username}
        onChange={(e) => setFormData({...formData, username: e.target.value})}
      />
      
      <input
        type="password"
        placeholder="Password"
        value={formData.password}
        onChange={(e) => setFormData({...formData, password: e.target.value})}
      />
      
      <button type="submit" disabled={loading}>
        {loading ? 'Fazendo login...' : 'Entrar'}
      </button>
    </form>
  );
}
```

## Gerenciamento de Token

### Armazenamento
```typescript
class TokenManager {
  private static readonly TOKEN_KEY = 'authToken';
  private static readonly REFRESH_KEY = 'refreshToken';

  static setTokens(accessToken: string, refreshToken?: string): void {
    localStorage.setItem(this.TOKEN_KEY, accessToken);
    if (refreshToken) {
      localStorage.setItem(this.REFRESH_KEY, refreshToken);
    }
  }

  static getAccessToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  static getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_KEY);
  }

  static clearTokens(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_KEY);
  }

  static isTokenValid(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.exp * 1000 > Date.now();
    } catch {
      return false;
    }
  }
}
```

### Interceptor para Requisições
```typescript
class ApiInterceptor {
  static async fetch(url: string, options: RequestInit = {}): Promise<Response> {
    const token = TokenManager.getAccessToken();
    
    if (token && TokenManager.isTokenValid(token)) {
      options.headers = {
        ...options.headers,
        'Authorization': `Bearer ${token}`
      };
    }

    const response = await fetch(url, options);

    if (response.status === 401) {
      TokenManager.clearTokens();
      window.location.href = '/login';
    }

    return response;
  }
}
```

## Localização (i18n)

### Arquivo de Mensagens (pt-BR)
```json
{
  "errors": {
    "1001": "Usuário não encontrado",
    "1002": "Usuário já existe",
    "1003": "Credenciais inválidas",
    "1004": "Usuário inativo",
    "1005": "Dados de entrada inválidos",
    "1006": "Erro de banco de dados",
    "1007": "Erro de validação"
  },
  "success": {
    "2001": "Usuário registrado com sucesso",
    "2002": "Usuário logado com sucesso",
    "2003": "Usuário recuperado com sucesso",
    "2004": "Operação concluída com sucesso"
  }
}
```

### Arquivo de Mensagens (en-US)
```json
{
  "errors": {
    "1001": "User not found",
    "1002": "User already exists",
    "1003": "Invalid credentials",
    "1004": "User is inactive",
    "1005": "Invalid input data",
    "1006": "Database error occurred",
    "1007": "Validation error"
  },
  "success": {
    "2001": "User registered successfully",
    "2002": "User logged in successfully",
    "2003": "User retrieved successfully",
    "2004": "Operation completed successfully"
  }
}
```

### Função de Localização
```typescript
function getLocalizedMessage(code: number, type: 'error' | 'success', locale: string): string {
  const messages = loadMessages(locale);
  const key = type === 'error' ? 'errors' : 'success';
  return messages[key][code.toString()] || `Unknown ${type}`;
}

// Uso
const errorMessage = getLocalizedMessage(1002, 'error', 'pt-BR'); // "Usuário já existe"
const successMessage = getLocalizedMessage(2001, 'success', 'en-US'); // "User registered successfully"
```

## Vantagens desta Abordagem

1. **Consistência**: Códigos padronizados em toda a aplicação
2. **Localização**: Fácil implementação de múltiplos idiomas
3. **Manutenibilidade**: Mudanças de mensagens sem alterar o backend
4. **Performance**: Códigos numéricos são mais eficientes que strings
5. **Debugging**: Códigos facilitam a identificação de problemas
6. **Escalabilidade**: Fácil adição de novos códigos
7. **Internacionalização**: Suporte nativo a múltiplos idiomas
8. **Segurança**: Autenticação JWT com claims padronizados
9. **Flexibilidade**: Endpoints protegidos e públicos bem definidos

## Boas Práticas

1. **Sempre verificar o código de erro** antes de exibir mensagens
2. **Implementar fallback** para códigos desconhecidos
3. **Usar constantes** para os códigos no frontend
4. **Implementar logging** para códigos de erro
5. **Criar testes** para diferentes cenários de erro
6. **Documentar** novos códigos adicionados
7. **Versionar** mudanças nos códigos de erro
8. **Validar tokens** antes de fazer requisições
9. **Implementar refresh token** para melhor UX
10. **Usar interceptors** para gerenciar headers automaticamente

## Status da API

✅ **Endpoints Disponíveis:**
- `POST /api/auth/register` - Registro de usuário
- `POST /api/auth/login` - Login de usuário
- `GET /api/auth/me` - Obter dados do usuário logado (protegido)

✅ **Funcionalidades:**
- Autenticação JWT funcionando
- Sistema CQRS limpo e funcional
- Validação de senhas (BCrypt + SHA256 fallback)
- Claims JWT (Name, Email, NameIdentifier)
- Logs de negócio apropriados
- Tratamento de erros com códigos enum
- Código limpo sem rotas de teste
