#拡大したHit&Blowの末路

##ヒット&ブローの概要

相手がどんな数字を想像しているかを当てる、二人で遊ぶゲームである。AさんBさんでプレイするとして説明する。

１，お互い、0～9の数字を4つ並べた数字を決める。同じ数字を複数回使ってはいけない。

２，Aさんは、Bさんがどんな数字を選んでいるか予測して宣言する。

３，Bさんは、Aさんが宣言した数字を以下のルールで判定して、その結果を伝える。
　　場所も数字もあっている数字は[H]、同じ数字を使っているが場所が違う場合は[B]とする。
  　（例）Bさんが[1357]を想像して、Aさんが[1234]を宣言した場合、BさんはAさんに対して「1H1Bです」と返答する事になります。

４，Bさんも同様にAさんの数字を予測して宣言、AさんはBさんの数字を判定して返答する。

５，これを「4H0B」になるまで繰り返す。

自動回答の簡単な実装について

最もシンプルな実装方法は以下のようなものになると考えられる
１，全パターンを配列やリストに保持する
２，最初はランダムや最初の値を適当に宣言する
３，判定結果から答えにならない組み合わせを削除し
４，残りのパターンから次に宣言するものを選ぶ
５，３と４を繰り返す
という流れ。

肝心なのは「３」の判定結果から、答えにならない組み合わせをどのように削除するかという事である。
答え[1357]に対して[1234]を宣言した場合を考えてみよう。
[1H1B]が返ってくるが、どれがHでどれがBであるかはわからないが、以下の事は言える。
「1234から二つ使用し、そのうち一つは場所も同じにする必要がある」
上の条件に合わないものは答えになりえないので削除する。
次に出せる数字として[0215] (1と2を使用し、2はそのままの位置、3と4は使わず、他の数を使う)を宣言する。
今度は[0H2B]となり、同様に答えにならない数字を削除する。

私の今回の実装について

上の方法はシンプルだが、最大値や使う数が増えると配列、リストの要素数が大きくなるという問題点がある。
0123～9876であれば大した事ではないが、今回割とまともな時間で計算できた「最大値19、使用する数8」だとしても
19P8、計算すると3047466240通りとなる。
2025年と還暦にちなんで「最大値2025 使用する数60」を考えていたため、家庭用PCでは到底かなわない数字になると予想された。

そこで、判定結果を組み合わせて毎回宣言する数字を計算する事にした。
以下にそのアルゴリズムを説明する。

答えは[1357]
A [1234]が[1H1B]だった場合には[1X2X] (Xは空いている場所を示す) などがあり得る
B [0215]が[0H2B]だった場合には[20XX] などがあり得る
この二つの判定結果が矛盾しない組み合わせを求めて、次に宣言する数字を計算してみる。
まず、使える数から考えてみよう
A → [1234]から二つ、残った二つは使えない
B → [0125]から二つ、残った二つは使えない
この条件を満たすのは以下の組み合わせ
A[12] B[12] [X]二つ
A[13] B[01] [X]一つ
A[14] B[01] [X]一つ
A[23] B[02] [X]一つ
A[23] B[25] [X]一つ
A[24] B[02] [X]一つ
A[24] B[25] [X]一つ
A[34] B[05] [X]一つ
という事なので、[12XX]を採用する。

次に並び順を考えてみよう。
Aは[1H1B]なので、どちらかは位置をそのままにする必要がある。
Bは[0H2B]なので、両方とも位置を変える必要がある。
Aで2の位置をそのままにすると、Bの[0H2B]に矛盾するので、1の位置をそのままにするしかない。
そうすると[1X2X]か[12XX]があり得る組み合わせとなるので、ここは[1X2X]を採用、Xには6と7を使って[1627]を宣言してみよう。
その判定結果は[2H0B]となる。

次は3件の判定結果が矛盾しない組み合わせを計算して宣言する。

この繰り返しで答えを求めるのが、私の考えたアルゴリズムである。
