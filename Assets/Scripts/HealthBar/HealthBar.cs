using UnityEngine;

//  Manage health bar graphics.
public class HealthBar : MonoBehaviour
{
	protected Enemy enemy;
	protected float maxHealth;
	protected SpriteRenderer redBarRenderer;
	protected SpriteRenderer greenBarRenderer;
	protected RectTransform greenBarRect;

	//  Set values.
	protected virtual void Awake()
	{
		enemy = transform.GetComponentInParent<Enemy>();
		maxHealth = enemy.health;
		redBarRenderer = transform.GetComponent<SpriteRenderer>();
		greenBarRenderer = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
		greenBarRect = transform.GetChild(0).GetComponent<RectTransform>();
	}

	//  Make the bar length correct.
	protected virtual void Update()
	{
		float ratio = enemy.health / maxHealth;
		if (ratio < 1)
			HealthBarUpdate(ratio);
	}

	//  Update health bar
	protected void HealthBarUpdate(float ratio)
	{
		if (ratio == 0)
			Destroy(gameObject);
		redBarRenderer.enabled = true;
		greenBarRenderer.enabled = true;
		greenBarRect.localScale = new Vector2(ratio, greenBarRect.localScale.y);
		greenBarRect.localPosition = new Vector2(-.5f + ratio / 2, greenBarRect.localPosition.y);
	}
}