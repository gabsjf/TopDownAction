<div align="center">

#  I Hate Orcs

**Jogo de ação 2D — enfrente hordas de orcs em combates rápidos e intensos**
> *Sem diplomacia. Sem negociações. Apenas uma missão: acabar com os orcs.*

</div>

---

##  Sobre o Jogo

Em um mundo constantemente ameaçado por invasões de orcs, um guerreiro solitário decide enfrentar a ameaça sozinho. Armado apenas com sua coragem e suas habilidades de combate, ele deve resistir ao avanço incessante dos inimigos e eliminar tudo o que surgir em seu caminho.

O foco do jogo é proporcionar uma experiência **simples, direta e divertida**, onde a sobrevivência depende da habilidade do jogador em enfrentar ondas cada vez mais perigosas de inimigos.

---

##  Gameplay

O jogador controla um personagem capaz de se movimentar livremente pelo cenário enquanto enfrenta inimigos que **perseguem e atacam constantemente**. À medida que o jogo avança, a dificuldade aumenta com o surgimento de novos inimigos e a crescente pressão sobre o jogador.

### Principais Mecânicas

| Mecânica | Descrição |
|---|---|
|  Movimentação | Controle em tempo real pelo cenário |
|  Combate | Ataques corpo a corpo diretos e responsivos |
|  Sistema de Vida | Gerenciamento de HP do jogador e inimigos |
|  IA Inimiga | Orcs com perseguição e ataque ao jogador |
|  Dificuldade Progressiva | Ondas cada vez mais intensas |
|  Reinício | Restart de partida após derrota |

---

##  Tecnologias

| Ferramenta | Uso |
|---|---|
| [Unity](https://unity.com) | Engine de desenvolvimento |
| [C#](https://docs.microsoft.com/dotnet/csharp) | Linguagem de programação |
| [Visual Studio](https://visualstudio.microsoft.com) | IDE |
| [Git](https://git-scm.com) & [GitHub](https://github.com) | Controle de versão |

---

##  Estrutura do Projeto

```
Assets
│
├── Scripts
│   ├── Player          # Controle, movimento e ataque do jogador
│   ├── Enemies         # IA, perseguição e comportamento dos orcs
│   ├── Combat          # Sistema de dano e combate
│   ├── UI              # Interface e HUD
│   ├── Managers        # Gerenciadores globais do jogo
│   └── Core            # Sistemas base reutilizáveis
│
├── Sprites             # Arte e sprites 2D
├── Animations          # Animações e controladores
├── Audio               # Trilha sonora e efeitos
├── Prefabs             # Objetos pré-configurados
└── Scenes              # Cenas e níveis
```

---

##  Sistemas Implementados

<table>
<tr>
<td width="50%">

###  Jogador
- Movimento e controle de direção
- Sistema de vida
- Recebimento de dano
- Sistema de ataque

###  Inimigos
- Spawn de orcs
- Perseguição do jogador
- Sistema de ataque
- Sistema de vida e morte

</td>
<td width="50%">

###  Gerenciamento
- Controle de partida
- Reinício de jogo
- Controle de inimigos ativos
- Gerenciamento de pontuação

###  Interface
- Exibição de vida
- Tela de derrota
- Indicadores de progresso

</td>
</tr>
</table>

---

##  Arquitetura

```
Player Input
      ↓
Player Controller
      ↓
Combat System
      ↓
Enemy System
      ↓
Game Manager
      ↓
UI Manager
```

> Essa separação permite que novos recursos sejam adicionados sem impactar diretamente sistemas já existentes.

---

##  Roadmap

### Concluído
- [x] Controle do jogador
- [x] Sistema de combate
- [x] IA básica dos inimigos
- [x] Sistema de vida
- [x] Sistema de derrota
- [x] Interface básica

### Futuras Melhorias
- [ ] Novos tipos de orcs
- [ ] Chefes especiais
- [ ] Sistema de habilidades
- [ ] Sistema de progressão
- [ ] Loja de upgrades
- [ ] Novas armas
- [ ] Efeitos visuais aprimorados
- [ ] Sistema de conquistas

---

##  Aprendizados

Este projeto serviu como laboratório para o desenvolvimento de sistemas fundamentais presentes em diversos gêneros de jogos:

- **Component-Based Architecture** — estrutura modular e reutilizável
- **Event-Driven Programming** — comunicação desacoplada entre sistemas
- **State Management** — controle de estados do jogador e inimigos
- **Object-Oriented Programming** — aplicação prática da orientação a objetos
- **Design Patterns** — padrões aplicados ao contexto de jogos
- **Organização de projetos Unity** — estrutura escalável e de fácil manutenção

---

##  Desafios Técnicos

- Implementação de IA de perseguição eficiente
- Controle de colisões entre múltiplos personagens simultâneos
- Balanceamento da dificuldade progressiva
- Sistema de spawn de inimigos com variação
- Feedback visual responsivo para dano e combate
- Organização do fluxo de jogo sem acoplamento excessivo

---

##  Possíveis Evoluções

Embora tenha começado como projeto de estudo, **I Hate Orcs** foi planejado de forma que novas funcionalidades possam ser incorporadas futuramente, transformando-o em uma experiência mais completa com elementos de:

- Progressão e builds de personagem
- Sobrevivência por ondas
- Upgrades e loja entre partidas

---

##  Autor

Desenvolvido por **Gabriel Jorge** como projeto de estudo em desenvolvimento de jogos com Unity e C#.

---

<div align="center">
  <sub>🪓 Porque alguns problemas só têm uma solução</sub>
</div>
