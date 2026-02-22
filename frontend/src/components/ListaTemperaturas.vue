<template>
  <div class="lista-card">
    <div class="lista-header">
      <h2>Todas as Temperaturas</h2>
      <button @click="carregar" :disabled="carregando" class="btn-atualizar">
        {{ carregando ? 'Atualizando...' : 'Atualizar' }}
      </button>
    </div>

    <div v-if="mensagem" class="mensagem erro">
      {{ mensagem }}
    </div>

    <div v-if="temperaturas.length === 0 && !carregando && !mensagem" class="vazio">
      Nenhuma temperatura registrada.
    </div>

    <table v-if="temperaturas.length > 0">
      <thead>
        <tr>
          <th>Cidade</th>
          <th>Temperatura</th>
          <th>Data</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="t in temperaturas" :key="t.id">
          <td>{{ t.nomeCidade }}</td>
          <td>{{ t.temperatura }}°C</td>
          <td>{{ formatarData(t.dataRegistro) }}</td>
          <td>
            <button @click="remover(t.id)" class="btn-remover">Remover</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { temperaturaService, type HistoricoTemperatura } from '@/services/temperaturaService'

const temperaturas = ref<HistoricoTemperatura[]>([])
const carregando = ref(false)
const mensagem = ref('')

function formatarData(data: string): string {
  return new Date(data).toLocaleString('pt-BR')
}

async function carregar() {
  carregando.value = true
  mensagem.value = ''

  try {
    temperaturas.value = await temperaturaService.obterTodas()
  } catch (error: any) {
    mensagem.value = error.response?.data?.erros?.[0] || 'Erro ao carregar temperaturas.'
  } finally {
    carregando.value = false
  }
}

async function remover(id: string) {
  try {
    await temperaturaService.remover(id)
    temperaturas.value = temperaturas.value.filter(t => t.id !== id)
  } catch (error: any) {
    mensagem.value = error.response?.data?.erros?.[0] || 'Erro ao remover temperatura.'
  }
}

onMounted(carregar)

defineExpose({ carregar })
</script>

<style scoped>
.lista-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.08);
  margin-top: 1.5rem;
}

.lista-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.lista-header h2 {
  color: #1a1a2e;
}

.btn-atualizar {
  padding: 0.5rem 1rem;
  background-color: #4a90d9;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
}

.btn-atualizar:hover {
  background-color: #357abd;
}

.btn-atualizar:disabled {
  background-color: #a0c4e8;
  cursor: not-allowed;
}

.btn-remover {
  padding: 0.3rem 0.8rem;
  background-color: #e74c3c;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-size: 0.85rem;
}

.btn-remover:hover {
  background-color: #c0392b;
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

.vazio {
  text-align: center;
  color: #999;
  padding: 2rem;
}

.mensagem.erro {
  padding: 0.75rem;
  border-radius: 8px;
  margin-bottom: 1rem;
  font-size: 0.9rem;
  text-align: center;
  background-color: #fee;
  color: #c0392b;
  border: 1px solid #f5c6cb;
}
</style>