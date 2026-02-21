<template>
  <div class="login-container">
    <div class="login-card">
      <h1>{{ isCadastro ? 'Criar Conta' : 'Login' }}</h1>

      <div v-if="mensagem" :class="['mensagem', mensagemTipo]">
        {{ mensagem }}
      </div>

      <form @submit.prevent="handleSubmit">
        <div class="campo">
          <label for="nome">Usuário</label>
          <input
            id="nome"
            v-model="nome"
            type="text"
            placeholder="Digite seu usuário"
            required
          />
        </div>

        <div class="campo">
          <label for="senha">Senha</label>
          <input
            id="senha"
            v-model="senha"
            type="password"
            placeholder="Digite sua senha"
            required
          />
        </div>

        <button type="submit" :disabled="carregando" class="btn-primario">
          {{ carregando ? 'Aguarde...' : isCadastro ? 'Cadastrar' : 'Entrar' }}
        </button>
      </form>

      <p class="alternar">
        {{ isCadastro ? 'Já tem conta?' : 'Não tem conta?' }}
        <a href="#" @click.prevent="isCadastro = !isCadastro">
          {{ isCadastro ? 'Fazer login' : 'Criar conta' }}
        </a>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { authService } from '@/services/authService'

const router = useRouter()

const nome = ref('')
const senha = ref('')
const carregando = ref(false)
const isCadastro = ref(false)
const mensagem = ref('')
const mensagemTipo = ref<'sucesso' | 'erro'>('erro')

async function handleSubmit() {
  carregando.value = true
  mensagem.value = ''

  try {
    if (isCadastro.value) {
      await authService.cadastrar({ nome: nome.value, senha: senha.value })
      mensagem.value = 'Cadastro realizado com sucesso! Faça login.'
      mensagemTipo.value = 'sucesso'
      isCadastro.value = false
      senha.value = ''
    } else {
      const token = await authService.login({ nome: nome.value, senha: senha.value })
      localStorage.setItem('token', token)
      router.push('/')
    }
  } catch (error: any) {
    mensagem.value = error.response?.data?.erros?.[0] || 'Erro ao processar requisição.'
    mensagemTipo.value = 'erro'
  } finally {
    carregando.value = false
  }
}
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background-color: #f0f2f5;
}

.login-card {
  background: white;
  padding: 2.5rem;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
}

.login-card h1 {
  text-align: center;
  margin-bottom: 1.5rem;
  color: #1a1a2e;
  font-size: 1.8rem;
}

.campo {
  margin-bottom: 1.2rem;
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
  transition: border-color 0.2s;
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
  transition: background-color 0.2s;
}

.btn-primario:hover {
  background-color: #357abd;
}

.btn-primario:disabled {
  background-color: #a0c4e8;
  cursor: not-allowed;
}

.alternar {
  text-align: center;
  margin-top: 1.2rem;
  color: #666;
  font-size: 0.9rem;
}

.alternar a {
  color: #4a90d9;
  text-decoration: none;
  font-weight: 500;
}

.alternar a:hover {
  text-decoration: underline;
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