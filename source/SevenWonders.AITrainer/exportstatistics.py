import pandas as pd
import matplotlib.pyplot as plt
import os

plt.figure(figsize=(12, 7))
smoothing_factor = 0.9

csv_files = ["Hard_First_Phase_0.csv", "Hard_Second_Phase_0.csv", "Hard_Third_Phase_0.csv", "Hard_Fourth_Phase_0.csv", "Hard_Fifth_Phase_0.csv", "Hard_Sixth_Phase_0.csv", "Hard_Final_Phase_0.csv"]

for file in csv_files:
    label_name = os.path.splitext(file)[0].replace('_', ' ')
    df = pd.read_csv(file)
    steps = df['Step']
    values = df['Value']
    smoothed_values = values.ewm(alpha=1 - smoothing_factor).mean()
    line, = plt.plot(steps, smoothed_values, label=label_name, linewidth=2)
    plt.plot(steps, values, color=line.get_color(), alpha=0.2)

plt.title('Learning Progress - Average Episode Reward')
plt.xlabel('Learning Steps (Steps)')
plt.ylabel('Average Reward (ep_rew_mean)')
plt.grid(True, linestyle='--', alpha=0.6)
plt.legend()

plt.tight_layout()
plt.savefig('Hard.png', dpi=300)
plt.show()
