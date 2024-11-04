## Curriculum Learning을 활용한 MMORPG 스타일의 보스전 자동화 에이전트 구현
---
#### 프로젝트 소개

* ML-Agent를 사용하여 MMORPG 게임을 플레이하는 AI를 구현해보았습니다.
  
* PPO 알고리즘과 Curriculum Learning 방법론을 사용하여 구현
  
* 복잡한 게임 환경을 설정하기 위해 MMORPG 레이드 환경을 구현
  * 보스는 11가지의 공격 스킬을 보유
 
  * Agent는 중거리, 단거리 스킬을 보유
 
---
#### 구조

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

---
#### 영상

* <https://youtu.be/watch?v=DT2CJnAG5Qo&t=1s>
