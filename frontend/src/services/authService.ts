import api from './api'

export interface LoginRequest {
  nome: string
  senha: string
}

export interface CadastroRequest {
  nome: string
  senha: string
}

export const authService = {
  async login(data: LoginRequest): Promise<string> {
    const response = await api.post('/usuarios/autenticar', data)
    return response.data.token
  },

  async cadastrar(data: CadastroRequest): Promise<void> {
    await api.post('/usuarios/criar-cadastro', data)
  }
}