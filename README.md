## Curriculum Learning을 활용한 MMORPG 스타일의 보스전 자동화 에이전트 구현

---
#### 영상

[![Video Label](http://img.youtube.com/vi/kNySQkXRHXE/0.jpg)](https://youtu.be/DT2CJnAG5Qo&t=1s)

---
#### 프로젝트 소개

* ML-Agent를 사용하여 MMORPG 게임을 플레이하는 AI를 구현해보았습니다.
  
* PPO 알고리즘과 Curriculum Learning 방법론을 사용하여 구현
  
* 복잡한 게임 환경을 설정하기 위해 MMORPG 레이드 환경을 구현
  * 보스는 11가지의 공격 스킬을 보유
 
  * Agent는 중거리, 단거리 스킬을 보유
 
* Agent는 ML-Agent를 통해 Agent의 이동, Boss의 위치, Boss 공격 스킬의 요소 등을 관측하고, 3D grid를 통해 3D 게임 환경 내 요소(Boss, 패턴, 벽 환경 등)의 위치와 Agent과의 관계를 관측합니다.
* 관측한 value를 토대로 Action(이동, 공격)을 하고 designed reward에 따라 reward를 습득합니다.(논문 참조)
* reward를 최대화하는 policy으로 근사하는 방향으로 학습한다.
* Boss는 InvokeRepeating와 Coroutine을 통해 일정 시간마다 Agent를 향해 공격을 시전합니다.
* Agent는 학습한 policy과 관측한 내용(Boss 위치, 공격 위치, 공격 종류 등)을 바탕으로 이동, 공격합니다.

![화면 캡처 2024-11-11 214508](https://github.com/user-attachments/assets/326f4e52-30be-4f37-8626-ca4d6c976a16)
---
#### 프로젝트 실행 방법

* Asset/Scene/MainScene을 실행하면 보스 레이드를 플레이하는 ai를 확인할 수 있습니다.
* MainScene내 존재하는 RPGPrefab의 Agent가 지닌 Behavior Parameter의 Model이 "RPG1-5000727"이면 PPO 알고리즘과 Curriculum learning 방법론을 결합하여 학습한 brain입니다.
* PPO 알고리즘만 사용한 brain을 경험하고 싶다면 "RPG-ppo_main"으로 교체하여 실행해주시면 됩니다.

* CompareCurriculum scene - Curriculum learning 방법론을 사용한 model과 사용하지 않은 model의 결과를 측정하기 위해 만든 scene입니다.
* CurriculumLearningScene, PPOLearningScene - ML Agent를 사용해 model을 학습시키기 위해 만든 scene입니다.

---
#### 논문
* Curriculum Learning을 활용한 MMORPG 스타일의 보스전 자동화 에이전트 구현 : <https://www.dbpia.co.kr/journal/articleDetail?nodeId=NODE11862417>
