<template>
  <div class="home-container">
    <header class="home-header">
      <h1>☁️ Consultar Tempo</h1>
      <nav class="nav-links">
        <a
          href="#"
          :class="{ ativo: abaAtiva === 'registrar' }"
          @click.prevent="abaAtiva = 'registrar'"
        >
          Registrar
        </a>
        <a
          href="#"
          :class="{ ativo: abaAtiva === 'temperaturas' }"
          @click.prevent="abaAtiva = 'temperaturas'"
        >
          Temperaturas
        </a>
        <a
          href="#"
          :class="{ ativo: abaAtiva === 'historico' }"
          @click.prevent="abaAtiva = 'historico'"
        >
          Histórico
        </a>
      </nav>
      <div class="user-info">
        <span>Olá, {{ nomeUsuario }}!</span>
        <button @click="logout" class="btn-sair">Sair</button>
      </div>
    </header>

    <main class="home-content">
      <div v-if="abaAtiva === 'registrar'">
        <RegistrarTemperatura @registrado="onRegistrado" />
      </div>

      <div v-if="abaAtiva === 'temperaturas'">
        <ListaTemperaturas ref="listaRef" />
      </div>

      <div v-if="abaAtiva === 'historico'">
        <HistoricoTemperaturas ref="historicoRef" />
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import RegistrarTemperatura from '@/components/RegistrarTemperatura.vue'
import ListaTemperaturas from '@/components/ListaTemperaturas.vue'
import HistoricoTemperaturas from '@/components/HistoricoTemperaturas.vue'

const router = useRouter()
const abaAtiva = ref<'registrar' | 'temperaturas' | 'historico'>('registrar')
const historicoRef = ref<InstanceType<typeof HistoricoTemperaturas> | null>(null)
const listaRef = ref<InstanceType<typeof ListaTemperaturas> | null>(null)
const nomeUsuario = ref('')

onMounted(() => {
  const token = localStorage.getItem('token')
  if (token) {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]))
      nomeUsuario.value = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || 'Usuário'
    } catch {
      nomeUsuario.value = 'Usuário'
    }
  }
})

function logout() {
  localStorage.removeItem('token')
  router.push('/login')
}

function onRegistrado() {
  listaRef.value?.carregar()
  historicoRef.value?.consultar()
}
</script>

<style scoped>
.home-container {
  min-height: 100vh;
  background-color: #f0f2f5;
}

.home-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.8rem 2rem;
  background-color: #1a1a2e;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}

.home-header h1 {
  color: white;
  font-size: 1.4rem;
  margin: 0;
}

.nav-links {
  display: flex;
  gap: 0.5rem;
}

.nav-links a {
  color: #a0aec0;
  text-decoration: none;
  padding: 0.5rem 1.2rem;
  border-radius: 8px;
  font-weight: 500;
  font-size: 0.95rem;
  transition: all 0.2s;
}

.nav-links a:hover {
  color: white;
  background-color: rgba(255, 255, 255, 0.1);
}

.nav-links a.ativo {
  color: white;
  background-color: #4a90d9;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.user-info span {
  color: #a0aec0;
  font-size: 0.9rem;
}

.btn-sair {
  padding: 0.4rem 1rem;
  background-color: transparent;
  color: #e74c3c;
  border: 1px solid #e74c3c;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  font-size: 0.85rem;
  transition: all 0.2s;
}

.btn-sair:hover {
  background-color: #e74c3c;
  color: white;
}

.home-content {
  max-width: 900px;
  margin: 2rem auto;
  padding: 0 1rem;
}
</style>