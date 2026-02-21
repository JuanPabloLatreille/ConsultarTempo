<template>
  <div class="historico-card">
    <h2>Histórico de Temperaturas</h2>

    <form @submit.prevent="consultar">
      <div class="campo">
        <label for="cidade-historico">Nome da Cidade</label>
        <input
          id="cidade-historico"
          v-model="nomeCidade"
          type="text"
          placeholder="Ex: Curitiba"
          required
        />
      </div>

      <button type="submit" :disabled="carregando" class="btn-primario">
        {{ carregando ? 'Consultando...' : 'Consultar Histórico' }}
      </button>
    </form>

    <div v-if="mensagem" :class="['mensagem', mensagemTipo]">
      {{ mensagem }}
    </div>

    <div v-if="historicos.length > 0" class="resultados">
      <div class="abas">
        <button
          :class="['aba', abaAtiva === 'lista' ? 'ativa' : '']"
          @click="abaAtiva = 'lista'"
        >
          Lista
        </button>
        <button
          :class="['aba', abaAtiva === 'grafico' ? 'ativa' : '']"
          @click="abaAtiva = 'grafico'"
        >
          Gráfico
        </button>
      </div>

      <div v-if="abaAtiva === 'lista'" class="lista">
        <table>
          <thead>
            <tr>
              <th>Data</th>
              <th>Temperatura</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="h in historicos" :key="h.id">
              <td>{{ formatarData(h.dataRegistro) }}</td>
              <td>{{ h.temperatura }}°C</td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-if="abaAtiva === 'grafico'" class="grafico">
        <Line :data="chartData" :options="chartOptions" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { Line } from 'vue-chartjs'
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
} from 'chart.js'
import { temperaturaService, type HistoricoTemperatura } from '@/services/temperaturaService'

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend)

const nomeCidade = ref('')
const carregando = ref(false)
const historicos = ref<HistoricoTemperatura[]>([])
const mensagem = ref('')
const mensagemTipo = ref<'sucesso' | 'erro'>('erro')
const abaAtiva = ref<'lista' | 'grafico'>('lista')

function formatarData(data: string): string {
  return new Date(data).toLocaleString('pt-BR')
}

async function consultar() {
  carregando.value = true
  mensagem.value = ''
  historicos.value = []

  try {
    historicos.value = await temperaturaService.consultarPorNome(nomeCidade.value)

    if (historicos.value.length === 0) {
      mensagem.value = 'Nenhum registro encontrado para esta cidade.'
      mensagemTipo.value = 'erro'
    }
  } catch (error: any) {
    mensagem.value = error.response?.data?.erros?.[0] || 'Erro ao consultar histórico.'
    mensagemTipo.value = 'erro'
  } finally {
    carregando.value = false
  }
}

const chartData = computed(() => ({
  labels: historicos.value
    .slice()
    .reverse()
    .map((h) => formatarData(h.dataRegistro)),
  datasets: [
    {
      label: 'Temperatura (°C)',
      data: historicos.value
        .slice()
        .reverse()
        .map((h) => h.temperatura),
      borderColor: '#4a90d9',
      backgroundColor: 'rgba(74, 144, 217, 0.1)',
      fill: true,
      tension: 0.3
    }
  ]
}))

const chartOptions = {
  responsive: true,
  plugins: {
    legend: {
      display: false
    }
  },
  scales: {
    y: {
      title: {
        display: true,
        text: '°C'
      }
    }
  }
}

defineExpose({ consultar })
</script>

<style scoped>
.historico-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.08);
  margin-top: 1.5rem;
}

.historico-card h2 {
  margin-bottom: 1rem;
  color: #1a1a2e;
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

.resultados {
  margin-top: 1.5rem;
}

.abas {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.aba {
  padding: 0.5rem 1.5rem;
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

table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  padding: 0.75rem;
  text-align: left;
  border-bottom: 1px solid #eee;
}

th {
  color: #666;
  font-weight: 600;
  font-size: 0.9rem;
}

td {
  color: #333;
}

.grafico {
  padding: 1rem 0;
}

.mensagem {
  padding: 0.75rem;
  border-radius: 8px;
  margin-top: 1rem;
  font-size: 0.9rem;
  text-align: center;
}

.mensagem.erro {
  background-color: #fee;
  color: #c0392b;
  border: 1px solid #f5c6cb;
}
</style>