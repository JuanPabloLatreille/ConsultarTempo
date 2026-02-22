<template>
  <div class="registrar-card">
    <h2>Registrar Temperatura</h2>

    <div v-if="mensagem" :class="['mensagem', mensagemTipo]">
      {{ mensagem }}
    </div>

    <div class="tipo-consulta">
      <button
        :class="['aba', tipo === 'nome' ? 'ativa' : '']"
        @click="tipo = 'nome'"
      >
        Por Nome
      </button>
      <button
        :class="['aba', tipo === 'coordenadas' ? 'ativa' : '']"
        @click="tipo = 'coordenadas'"
      >
        Por Coordenadas
      </button>
    </div>

    <form @submit.prevent="registrar">
      <div v-if="tipo === 'nome'" class="campo">
        <label for="cidade">Nome da Cidade</label>
        <input
          id="cidade"
          v-model="nomeCidade"
          type="text"
          placeholder="Ex: Toledo"
          required
        />
      </div>

      <div v-if="tipo === 'coordenadas'">
        <div class="campo">
          <label for="latitude">Latitude</label>
          <input
            id="latitude"
            v-model.number="latitude"
            type="number"
            step="any"
            placeholder="Ex: -24.7253"
            required
          />
        </div>
        <div class="campo">
          <label for="longitude">Longitude</label>
          <input
            id="longitude"
            v-model.number="longitude"
            type="number"
            step="any"
            placeholder="Ex: -53.7412"
            required
          />
        </div>
      </div>

      <button type="submit" :disabled="carregando" class="btn-primario">
        {{ carregando ? 'Consultando...' : 'Registrar Temperatura' }}
      </button>
    </form>

    <div v-if="resultado" class="resultado">
      <p><strong>Cidade:</strong> {{ resultado.nomeCidade }}</p>
      <p><strong>Temperatura:</strong> {{ resultado.temperaturaAtual }}°C</p>
      <p><strong>Data:</strong> {{ formatarData(resultado.dataRegistro) }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { temperaturaService, type RegistroTemperatura } from '@/services/temperaturaService'

const emit = defineEmits(['registrado'])

const tipo = ref<'nome' | 'coordenadas'>('nome')
const nomeCidade = ref('')
const latitude = ref<number | null>(null)
const longitude = ref<number | null>(null)
const carregando = ref(false)
const resultado = ref<RegistroTemperatura | null>(null)
const mensagem = ref('')
const mensagemTipo = ref<'sucesso' | 'erro'>('erro')

function formatarData(data: string): string {
  return new Date(data).toLocaleString('pt-BR')
}

async function registrar() {
  carregando.value = true
  mensagem.value = ''
  resultado.value = null

  try {
    if (tipo.value === 'nome') {
      resultado.value = await temperaturaService.registrarPorNome(nomeCidade.value)
    } else {
      resultado.value = await temperaturaService.registrarPorCoordenadas(latitude.value!, longitude.value!)
    }
    mensagem.value = 'Temperatura registrada com sucesso!'
    mensagemTipo.value = 'sucesso'
    emit('registrado')
  } catch (error: any) {
    mensagem.value = error.response?.data?.erros?.[0] || 'Erro ao registrar temperatura.'
    mensagemTipo.value = 'erro'
  } finally {
    carregando.value = false
  }
}
</script>

<style scoped>
.registrar-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.08);
}

.registrar-card h2 {
  margin-bottom: 1rem;
  color: #1a1a2e;
}

.tipo-consulta {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.aba {
  padding: 0.5rem 1.2rem;
  border: 1px solid #ddd;
  background: white;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  color: #666;
}

.aba.ativa {
  background-color: #4a90d9;
  color: white;
  border-color: #4a90d9;
}

.campo {
  margin-bottom: 1rem;
}

.campo label {
  display: block;
  margin-bottom: 0.4rem;
  color: #333;
  font-weight: 500;
  font-size: 0.9rem;
}

.campo input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-size: 1rem;
  box-sizing: border-box;
}

.campo input:focus {
  outline: none;
  border-color: #4a90d9;
}

.btn-primario {
  width: 100%;
  padding: 0.75rem;
  background-color: #4a90d9;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-primario:hover {
  background-color: #357abd;
}

.btn-primario:disabled {
  background-color: #a0c4e8;
  cursor: not-allowed;
}

.resultado {
  margin-top: 1rem;
  padding: 1rem;
  background-color: #eafaf1;
  border-radius: 8px;
  border: 1px solid #a9dfbf;
}

.resultado p {
  margin: 0.3rem 0;
  color: #333;
}

.mensagem {
  padding: 0.75rem;
  border-radius: 8px;
  margin-bottom: 1rem;
  font-size: 0.9rem;
  text-align: center;
}

.mensagem.erro {
  background-color: #fee;
  color: #c0392b;
  border: 1px solid #f5c6cb;
}

.mensagem.sucesso {
  background-color: #eafaf1;
  color: #27ae60;
  border: 1px solid #a9dfbf;
}
</style>