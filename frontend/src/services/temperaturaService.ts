import api from './api'

export interface HistoricoTemperatura {
  id: string
  nomeCidade: string
  temperatura: number
  dataRegistro: string
}

export interface RegistroTemperatura {
  id: string
  cidadeId: string
  nomeCidade: string
  temperaturaAtual: number
  dataRegistro: string
}

export const temperaturaService = {
  async registrarPorNome(nomeCidade: string, codigoPais: string = 'BR'): Promise<RegistroTemperatura> {
    const response = await api.post('/historicostemperaturas/por-nome', { nomeCidade, codigoPais })
    return response.data
  },

  async registrarPorCoordenadas(latitude: number, longitude: number): Promise<RegistroTemperatura> {
  const response = await api.post('/historicostemperaturas/por-coordenadas', { latitude, longitude })
  return response.data
  },

  async consultarPorNome(nomeCidade: string): Promise<HistoricoTemperatura[]> {
    const response = await api.get(`/historicostemperaturas/por-nome/${nomeCidade}`)
    return response.data
  },

  async obterTodas(): Promise<HistoricoTemperatura[]> {
    const response = await api.get('/historicostemperaturas')
    return response.data
  },

  async remover(id: string): Promise<void> {
    await api.delete(`/historicostemperaturas/id/${id}`)
  }
}